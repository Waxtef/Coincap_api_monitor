using Coincap.PriceHistory.Application.Queries.GetPriceHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coincap.API.Controllers;

[ApiController]
[Route("api/assets/{assetId}/history")]
public class PriceHistoryController : ControllerBase
{
    private readonly ISender _sender;

    public PriceHistoryController(ISender sender)
    {
        _sender = sender;
    }

    // GET /api/assets/'currency'/history'from=2024-01-01_to=2024-01-31'
    [HttpGet]
    public async Task<IActionResult> GetHistory(
        string assetId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(new GetPriceHistoryQuery(assetId, from, to), ct);
        return Ok(result);
    }
}
