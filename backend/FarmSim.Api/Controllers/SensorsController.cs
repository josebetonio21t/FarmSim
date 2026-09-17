using System.ComponentModel.DataAnnotations;
using FarmSim.Api.Contracts;
using FarmSim.Api.Models;
using FarmSim.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmSim.Api.Controllers;

[ApiController]
[Route("api/sensors")]
public sealed class SensorsController(ISensorService sensorService) : ControllerBase
{
    [HttpGet("current")]
    [ProducesResponseType<SensorReading>(StatusCodes.Status200OK)]
    public ActionResult<SensorReading> GetCurrent() => Ok(sensorService.GetCurrent());

    [HttpPost("simulate")]
    [ProducesResponseType<SensorReading>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<SensorReading> Simulate(SimulationRequest request)
    {
        var reading = sensorService.Simulate(request);
        return CreatedAtAction(nameof(GetCurrent), reading);
    }

    [HttpGet("history")]
    [ProducesResponseType<IReadOnlyList<SensorReading>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<IReadOnlyList<SensorReading>> GetHistory(
        [FromQuery, Range(1, 100)] int limit = 25) =>
        Ok(sensorService.GetHistory(limit));
}
