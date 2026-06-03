using Coincap.Assets.Application.Queries.GetAssetById;
using Coincap.Assets.Application.Queries.GetAssets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coincap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly ISender _sender;

    public AssetsController(ISender sender)
    {
        _sender = sender;
    }

    // GET /api/assets?page=1&pageSize=20&search=bitcoin
    [HttpGet]
    public async Task<IActionResult> GetAssets(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(new GetAssetsQuery(page, pageSize, search), ct);
        return Ok(result);
    }

    // GET /api/assets/'name of currency'
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetById(string id, CancellationToken ct = default)
    {
        var result = await _sender.Send(new GetAssetByIdQuery(id), ct);

        // Si el handler retornó Failure lo convertimos en 404
        if (result.IsFailure)
            return NotFound(new { message = result.Error });

        return Ok(result.Value);
    }
}
