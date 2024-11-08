using Microsoft.EntityFrameworkCore;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.UserModule.Business.Data;
using SchoolProject.UserModule.Business.Repositories;
using SchoolProject.UserModule.Business.Repositories.Interfaces;
using SchoolProject.UserModule.Business.Services;
using SchoolProject.UserModule.Business.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

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

app.MapControllers();

app.Run();
