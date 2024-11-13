using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.UserModule.Api.Filter;
using SchoolProject.UserModule.Api.Validators;
using SchoolProject.UserModule.API.ExceptionHandler;
using SchoolProject.UserModule.Business.Data;
using SchoolProject.UserModule.Business.Repositories;
using SchoolProject.UserModule.Business.Repositories.Interfaces;
using SchoolProject.UserModule.Business.Services;
using SchoolProject.UserModule.Business.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = true);
builder.Services.AddScoped<ModelValidationFilter>();
builder.Services.AddControllers(options => options.Filters.Add<ModelValidationFilter>());
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new() { Title = "UserModule API", Version = "v1" });
});

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var serverVersion = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Localhost"));
builder.Services.AddDbContext<UserModuleDbContext>(options => 
{
    options.UseMySql(builder.Configuration.GetConnectionString("Localhost"), serverVersion)
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseExceptionHandler(_ => { });

app.MapControllers();

app.Run();
