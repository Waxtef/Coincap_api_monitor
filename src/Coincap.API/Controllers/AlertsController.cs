using Coincap.Alerts.Application.Queries.GetAlerts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coincap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly ISender _sender;

    public AlertsController(ISender sender)
    {
        _sender = sender;
    }

    // GET /api/alerts?assetId=bitcoin
    [HttpGet]
    public async Task<IActionResult> GetAlerts(
        [FromQuery] string? assetId = null,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(new GetAlertsQuery(assetId), ct);
        return Ok(result);
    }
}
