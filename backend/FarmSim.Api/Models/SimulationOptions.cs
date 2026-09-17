namespace FarmSim.Api.Models;

public sealed class SimulationOptions
{
    public const string SectionName = "Simulation";

    public int MaximumSensorHistory { get; init; } = 500;
}
