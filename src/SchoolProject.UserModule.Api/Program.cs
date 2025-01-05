using System.Reflection;
using Consul;
using FluentValidation;
using SchoolProject.Core.Business;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.UserModule.Api.Validators;
using SchoolProject.UserModule.Business.Data;
using SchoolProject.UserModule.Business.Repositories;
using SchoolProject.UserModule.Business.Repositories.Interfaces;
using SchoolProject.UserModule.Business.Services;
using SchoolProject.UserModule.Business.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
builder.Services.AddSwagger(builder.Configuration, xmlPath);
builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddExceptionHandling();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDbContextRef<UserModuleDbContext>(builder.Configuration);

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register with Consul
var consulClient = new ConsulClient(config => config.Address = new Uri("http://localhost:8500"));
var serviceName = "authService";
var serviceId = $"{serviceName}-{Guid.NewGuid()}";

var serviceUri = new Uri(app.Urls.FirstOrDefault() ?? "http://localhost:5000");
var registration = new AgentServiceRegistration
{
    ID = serviceId,
    Name = serviceName,
    Address = serviceUri.Host,
    Port = serviceUri.Port,
    Check = new AgentServiceCheck
    {
        HTTP = $"{serviceUri.Scheme}://{serviceUri.Host}:{serviceUri.Port}/health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
    }
};

// Register the service with Consul
await consulClient.Agent.ServiceRegister(registration);

app.Lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(serviceId).Wait();
});

// Health Check Endpoint
app.MapGet("/health", () => Results.Ok("Auth Service is healthy"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
