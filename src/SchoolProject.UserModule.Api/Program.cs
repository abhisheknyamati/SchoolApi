using System.Reflection;
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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
