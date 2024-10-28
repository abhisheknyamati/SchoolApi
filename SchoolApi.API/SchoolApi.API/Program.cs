using SchoolApi.API.Mappers;
using SchoolApi.Business.Repositories; 
using SchoolApi.Business.Services;
using SchoolApi.Business.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var serverVersion = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Localhost"));
builder.Services.AddDbContext<SchoolAPIDbContext>(options =>
           options.UseMySql(builder.Configuration.GetConnectionString("Localhost"), serverVersion,
               b => b.MigrationsAssembly("SchoolApi.API"))
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging());


var app = builder.Build();

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