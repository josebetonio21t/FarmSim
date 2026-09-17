using System.ComponentModel.DataAnnotations;

namespace FarmSim.Api.Contracts;

public sealed class UpdateActuatorRequest
{
    public bool IsOn { get; init; }

    [Range(0, 100)]
    public int PowerPercent { get; init; } = 100;
}
