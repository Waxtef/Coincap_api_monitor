using Coincap.ExternalAPI.Application.Commands.SyncAssets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coincap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly ISender _sender;

    public SyncController(ISender sender)
    {
        _sender = sender;
    }

    // POST /api/sync — dispara sincronización manual con CoinCap
    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken ct = default)
    {
        var result = await _sender.Send(new SyncAssetsCommand(), ct);
        return Ok(new
        {
            message = "Sincronización completada",
            assetsProcessed = result.AssetsProcessed,
            syncedAt = result.SyncedAt
        });
    }
}
