namespace FarmSim.Api.Models;

/// <summary>The latest simulated state of a controllable hydroponics device.</summary>
public sealed record ActuatorState(
    string Name,
    bool IsOn,
    int PowerPercent,
    DateTimeOffset UpdatedAtUtc);
