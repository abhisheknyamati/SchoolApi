using FluentValidation;
using Serilog;
using SchoolProject.StudentModule.Business.Data;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Repositories;
using SchoolProject.StudentModule.Business.Services;
using SchoolProject.StudentModule.Business.Services.Interfaces;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.StudentModule.Api.Validators;
using SchoolProject.Core.Business;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using System.Net;
using MediatR;
using SchoolProject.StudentModule.Api.Handlers;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.Core.Business.Repositories;
using SchoolProject.Core.Business.Services.Interfaces;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using SchoolProject.StudentModule.Api.Listener;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .Enrich.FromLogContext()
//     .Enrich.WithExceptionDetails()
//     // .WriteTo.Console(new JsonFormatter())
//     .WriteTo.File(new JsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Day)
//     .CreateLogger();
// builder.Host.UseSerilog();

builder.Services.AddEmailService();
builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
builder.Services.AddSingleton<Plain.RabbitMQ.IPublisher>( p => new Publisher(
    p.GetRequiredService<IConnectionProvider>(),
    "school.exchange",
    ExchangeType.Topic
));
builder.Services.AddSingleton<ISubscriber>(s => new Subscriber(
    s.GetService<IConnectionProvider>(),
    "student.exchange",  // The exchange to subscribe to
    "student.event.queue",   // The queue name
    "student.*",   // The routing key to listen to
    ExchangeType.Topic 
));
builder.Services.AddHostedService<StudentEventEmailListener>();

builder.Services.AddMediatR(typeof(GetStudentListHandler).Assembly);
 
builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration, xmlPath);
builder.Services.AddExceptionHandling();
builder.Services.AddJwtAuthentication(builder.Configuration);
// builder.Services.AddDbContextRef<StudentModuleDbContext>(builder.Configuration);
builder.Services.AddDbContextReadWriteRef<StudentModuleDbContext>(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureHealthChecks(builder.Configuration);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IStudentService, StudentService>();
// builder.Services.AddScoped<IEmailService, 

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

app.MapHealthChecks("/api/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(delegate (Options options) 
{
    options.UIPath = "/healthcheck-ui";
    options.ApiPath = "/healthcheck-api";
});

app.MapControllers();

app.Run();
