using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SchoolProject.Core.Business.ExceptionHandler;
using SchoolProject.Core.Business.Filter;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SchoolProject.Core.Business.HealthChecker;
using SchoolProject.Core.Business.Services.Interfaces;

namespace SchoolProject.Core.Business
{
    public static class CommonServices
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(configuration["Swagger:Version"], new OpenApiInfo { Title = configuration["Swagger:Title"], Version = configuration["Swagger:Version"] });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddControllers(options =>
            {
                options.Filters.Add<ModelValidationFilter>();
            });

            services.AddEndpointsApiExplorer();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = true);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        RoleClaimType = "Role",

                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = async context =>
                        {
                            var exception = context.Exception;

                            if (exception is SecurityTokenExpiredException)
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                throw new SecurityTokenExpiredException(ErrorMsgConstant.UnauthorizedAccessException);
                            }
                            else if (exception is SecurityTokenInvalidSignatureException)
                            {
                                throw new InvalidSignature(ErrorMsgConstant.InvalidSignature);
                            }
                            else if (exception is SecurityTokenMalformedException)
                            {
                                throw new InvalidFormat(ErrorMsgConstant.InvalidFormat);
                            }

                            throw new Exception(ErrorMsgConstant.InternalError);
                        }
                    };
                });

        }

        public static void AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        public static void AddDbContextRef<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            var serverVersion = ServerVersion.AutoDetect(configuration.GetConnectionString("Localhost"));
            services.AddDbContext<T>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("Localhost"), serverVersion)
                       .EnableDetailedErrors()
                       .EnableSensitiveDataLogging();
            });
        }

        public static void AddDbContextReadWriteRef<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            string connectionString;
            services.AddDbContext<T>((serviceProvider, options) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                if (httpContextAccessor.HttpContext?.Request.Method == HttpMethods.Get)
                {
                    connectionString = configuration.GetConnectionString("SlaveDB");
                }
                else
                {
                    connectionString = configuration.GetConnectionString("MasterDB");
                }
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                options.UseMySql(connectionString, serverVersion)
                       .EnableDetailedErrors()
                       .EnableSensitiveDataLogging();
            });
        }

        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddMySql(configuration["ConnectionStrings:Localhost"], healthQuery: "select 1", name: "SQL servere",
                failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Database" })
                .AddCheck<RemoteHealthCheck>("Remote endpoints Health Check", failureStatus: HealthStatus.Unhealthy)
                .AddCheck<MemoryHealthCheck>($"Feedback Service Memory Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback Service" });
            // .AddUrlGroup(new Uri("https://localhost:5207/api/v1/heartbeats/ping"), name: "base URL", failureStatus: HealthStatus.Unhealthy);

            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(20); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("feedback api", "http://localhost:5207/api/health"); //map health check api    
            })
            // .AddMySqlStorage(configuration["ConnectionStrings:HealthChecksUI"]);
            .AddInMemoryStorage();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }
    
        public static void AddEmailService(this IServiceCollection services)
        {
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}