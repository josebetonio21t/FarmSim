using FarmSim.Api.Models;

namespace FarmSim.Api.Services;

public sealed class ActuatorService : IActuatorService
{
    private readonly Lock _sync = new();
    private readonly Dictionary<string, ActuatorState> _states;

    public ActuatorService()
    {
        var createdAt = DateTimeOffset.UtcNow;
        _states = new Dictionary<string, ActuatorState>(StringComparer.OrdinalIgnoreCase)
        {
            ["water-pump"] = new("water-pump", false, 0, createdAt),
            ["grow-light"] = new("grow-light", false, 0, createdAt),
            ["aerator"] = new("aerator", true, 100, createdAt),
            ["nutrient-doser"] = new("nutrient-doser", false, 0, createdAt)
        };
    }

    public IReadOnlyList<ActuatorState> GetAll()
    {
        lock (_sync)
        {
            return _states.Values.OrderBy(state => state.Name).ToArray();
        }
    }

    public ActuatorState? SetState(string name, bool isOn, int powerPercent)
    {
        lock (_sync)
        {
            if (!_states.ContainsKey(name))
            {
                return null;
            }

            var updated = new ActuatorState(
                name.ToLowerInvariant(),
                isOn,
                isOn ? powerPercent : 0,
                DateTimeOffset.UtcNow);

            _states[name] = updated;
            return updated;
        }
    }
}
