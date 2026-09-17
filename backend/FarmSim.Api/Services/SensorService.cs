using FarmSim.Api.Contracts;
using FarmSim.Api.Models;
using Microsoft.Extensions.Options;

namespace FarmSim.Api.Services;

public sealed class SensorService : ISensorService
{
    private readonly Lock _sync = new();
    private readonly Queue<SensorReading> _history = new();
    private readonly int _maximumHistorySize;

    public SensorService(IOptions<SimulationOptions> options)
    {
        _maximumHistorySize = options.Value.MaximumSensorHistory;
    }

    public SensorReading GetCurrent() => CreateAndStore(null);

    public SensorReading Simulate(SimulationRequest request) => CreateAndStore(request);

    public IReadOnlyList<SensorReading> GetHistory(int limit)
    {
        lock (_sync)
        {
            return _history.Reverse().Take(limit).ToArray();
        }
    }

    private SensorReading CreateAndStore(SimulationRequest? request)
    {
        var reading = new SensorReading(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            request?.Ph ?? NextDouble(5.8, 6.5),
            request?.WaterTemperatureC ?? NextDouble(20.0, 24.0),
            request?.AirTemperatureC ?? NextDouble(22.0, 29.0),
            request?.HumidityPercent ?? NextDouble(55.0, 75.0),
            request?.ElectricalConductivityMsCm ?? NextDouble(1.2, 2.4),
            request?.WaterLevelPercent ?? NextDouble(65.0, 100.0));

        lock (_sync)
        {
            _history.Enqueue(reading);

            while (_history.Count > _maximumHistorySize)
            {
                _history.Dequeue();
            }
        }

        return reading;
    }

    private static double NextDouble(double minimum, double maximum) =>
        Math.Round(minimum + (Random.Shared.NextDouble() * (maximum - minimum)), 2);
}
