using FarmSim.Api.Models;

namespace FarmSim.Api.Services;

public interface IActuatorService
{
    IReadOnlyList<ActuatorState> GetAll();
    ActuatorState? SetState(string name, bool isOn, int powerPercent);
}
