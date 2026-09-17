namespace FarmSim.Api.Models;

/// <summary>A simulated snapshot of the hydroponics environment.</summary>
public sealed record SensorReading(
    Guid Id,
    DateTimeOffset RecordedAtUtc,
    double Ph,
    double WaterTemperatureC,
    double AirTemperatureC,
    double HumidityPercent,
    double ElectricalConductivityMsCm,
    double WaterLevelPercent);
