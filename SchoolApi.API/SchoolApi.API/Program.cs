using SchoolApi.API.Mappers;
using SchoolApi.Business.Repositories; 
using SchoolApi.Business.Services;
using SchoolApi.Business.Data;
using Microsoft.EntityFrameworkCore;
using SchoolApi.API.ExceptionHandler;
using FluentValidation.AspNetCore;
using SchoolApi.API.Validators;
using SchoolApi.API.DTOs;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Customize model state invalid response
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "This field is required.");
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var traceId = Guid.NewGuid(); // Generate a new Trace ID for each error response
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
            Errors = errors // Assuming you add a property to ErrorDetails to hold validation errors
        };

        return new BadRequestObjectResult(errorDetails);
    };
});

// Register your repositories and services
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

// Use AddFluentValidation to automatically validate
// builder.Services.AddFluentValidation(config => 
// {
//     config.RegisterValidatorsFromAssemblyContaining<StudentValidator>();
//     config.RegisterValidatorsFromAssemblyContaining<StudentUpdateValidator>();
// });

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
builder.Services.AddDbContext<SchoolAPIDbContext>(options => 
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

// Use the global exception handler middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
