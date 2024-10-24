using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models.AutomapperHelper;
using SchoolApi.Repositories;
using SchoolApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var serverVersion = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Localhost"));
builder.Services.AddDbContext<SchoolAPIDbContext>(
    options => options
    .UseMySql(builder.Configuration.GetConnectionString("Localhost"), serverVersion)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
