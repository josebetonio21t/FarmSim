using FarmSim.Api.Contracts;
using FarmSim.Api.Models;

namespace FarmSim.Api.Services;

public interface ISensorService
{
    SensorReading GetCurrent();
    SensorReading Simulate(SimulationRequest request);
    IReadOnlyList<SensorReading> GetHistory(int limit);
}
