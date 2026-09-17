using System.ComponentModel.DataAnnotations;

namespace FarmSim.Api.Contracts;

/// <summary>
/// Optional sensor overrides. Omitted values are generated inside realistic ranges.
/// </summary>
public sealed class SimulationRequest
{
    [Range(0, 14)]
    public double? Ph { get; init; }

    [Range(0, 60)]
    public double? WaterTemperatureC { get; init; }

    [Range(-20, 70)]
    public double? AirTemperatureC { get; init; }

    [Range(0, 100)]
    public double? HumidityPercent { get; init; }

    [Range(0, 10)]
    public double? ElectricalConductivityMsCm { get; init; }

    [Range(0, 100)]
    public double? WaterLevelPercent { get; init; }
}
