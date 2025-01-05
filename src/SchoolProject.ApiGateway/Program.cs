using Consul;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json")
    .Build();

builder.Services.AddSwaggerForOcelot(configuration);
builder.Services.AddOcelot(configuration).AddConsul();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var consulClient = new ConsulClient(config => config.Address = new Uri("http://localhost:8500"));
var serviceName = "gateway";
var serviceId = $"{serviceName}-{Guid.NewGuid()}";

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("Gateway is healthy"));
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unhandled exception: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred in the gateway.");
    }
});

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

app.UseHttpsRedirection();

// Register gateway with Consul
var registration = new AgentServiceRegistration
{
    ID = serviceId,
    Name = serviceName,
    Address = "localhost",
    Port = 7000,
    Check = new AgentServiceCheck
    {
        HTTP = "http://localhost:7000/health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
    }
};

await consulClient.Agent.ServiceRegister(registration);
app.Lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(serviceId).Wait();
});

app.Run();
