using FarmSim.Api.Contracts;
using FarmSim.Api.Models;
using FarmSim.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmSim.Api.Controllers;

[ApiController]
[Route("api/actuators")]
public sealed class ActuatorsController(IActuatorService actuatorService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ActuatorState>>(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ActuatorState>> GetAll() =>
        Ok(actuatorService.GetAll());

    [HttpPost("{name}")]
    [ProducesResponseType<ActuatorState>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<ActuatorState> SetState(
        [FromRoute] string name,
        UpdateActuatorRequest request)
    {
        var updated = actuatorService.SetState(name, request.IsOn, request.PowerPercent);

        if (updated is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Unknown actuator",
                Detail = $"Actuator '{name}' does not exist. Use GET /api/actuators to list valid names."
            });
        }

        return Ok(updated);
    }
}
