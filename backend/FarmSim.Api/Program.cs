using FarmSim.Api.Infrastructure;
using FarmSim.Api.Models;
using FarmSim.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services
    .AddOptions<SimulationOptions>()
    .BindConfiguration(SimulationOptions.SectionName)
    .Validate(
        options => options.MaximumSensorHistory is >= 1 and <= 10_000,
        "Simulation:MaximumSensorHistory must be between 1 and 10000.")
    .ValidateOnStart();
builder.Services.AddSingleton<ISensorService, SensorService>();
builder.Services.AddSingleton<IActuatorService, ActuatorService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<RequestCorrelationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();

public partial class Program;
