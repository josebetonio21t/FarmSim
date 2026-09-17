using Microsoft.AspNetCore.Mvc;

namespace FarmSim.Api.Controllers;

[ApiController]
[Route("health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new
    {
        status = "healthy",
        service = "FarmSim.Api",
        version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
        storage = "in-memory simulation",
        utcTime = DateTimeOffset.UtcNow
    });
}
