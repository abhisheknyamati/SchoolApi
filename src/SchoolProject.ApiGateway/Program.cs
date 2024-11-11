using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json")
    .Build();

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseHttpsRedirection();

await app.UseOcelot();

await app.RunAsync();

