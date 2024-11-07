using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Serilog;
using SchoolProject.StudentModule.Business.Data;
using SchoolProject.StudentModule.API.DTOs;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Repositories;
using SchoolProject.StudentModule.Business.Services;
using SchoolProject.StudentModule.Business.Services.Interfaces;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.StudentModule.Api.Validators;
using SchoolProject.StudentModule.API.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

builder.Services.AddControllers(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "This field is required.");
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var traceId = Guid.NewGuid(); 
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(err => err.ErrorMessage).ToArray()
            );

        var errorDetails = new ErrorDetails
        {
            TraceId = traceId,
            Message = "One or more validation errors occurred.",
            StatusCode = (int)HttpStatusCode.BadRequest,
            Instance = context.HttpContext.Request.Path,
            ExceptionMessage = "Validation failed.",
            Errors = errors 
        };

        return new BadRequestObjectResult(errorDetails);
    };
});

builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentUpdateValidator>();

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
builder.Services.AddDbContext<StudentModuleDbContext>(options => 
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

app.UseCors("AllowAllOrigins");

app.UseExceptionHandler(_=>{ });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
