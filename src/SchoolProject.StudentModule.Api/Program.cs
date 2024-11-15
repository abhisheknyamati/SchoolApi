using FluentValidation;
using Serilog;
using SchoolProject.StudentModule.Business.Data;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Repositories;
using SchoolProject.StudentModule.Business.Services;
using SchoolProject.StudentModule.Business.Services.Interfaces;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.StudentModule.Api.Validators;
using SchoolProject.StudentModule.API.ExceptionHandler;
using SchoolProject.Core.Business;

var builder = WebApplication.CreateBuilder(args);

// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .Enrich.FromLogContext()
//     .Enrich.WithExceptionDetails()
//     // .WriteTo.Console(new JsonFormatter())
//     .WriteTo.File(new JsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Day)
//     .CreateLogger();
// builder.Host.UseSerilog();


builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddExceptionHandling();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDbContextRef<StudentModuleDbContext>(builder.Configuration);

builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentUpdateValidator>();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
