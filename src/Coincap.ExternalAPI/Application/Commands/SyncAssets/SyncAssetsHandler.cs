using Coincap.Assets.Application.Commands.SaveAsset;
using Coincap.ExternalAPI.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Coincap.ExternalAPI.Application.Commands.SyncAssets;

// Orquesta la sincronización completa: trae datos de CoinCap y guarda cada activo
public class SyncAssetsHandler : IRequestHandler<SyncAssetsCommand, SyncAssetsResult>
{
    private readonly CoinCapClient _client;
    private readonly ISender _sender;
    private readonly ILogger<SyncAssetsHandler> _logger;

    public SyncAssetsHandler(CoinCapClient client, ISender sender, ILogger<SyncAssetsHandler> logger)
    {
        _client = client;
        _sender = sender;
        _logger = logger;
    }

    public async Task<SyncAssetsResult> Handle(SyncAssetsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando sincronización con CoinCap");

        var assets = await _client.GetAssetsAsync(cancellationToken);

        foreach (var asset in assets)
        {
            // Convierte los strings de CoinCap a decimal — si viene null o inválido usa 0
            _ = decimal.TryParse(asset.PriceUsd, out var priceUsd);
            _ = decimal.TryParse(asset.MarketCapUsd, out var marketCapUsd);
            _ = decimal.TryParse(asset.VolumeUsd24Hr, out var volumeUsd24Hr);
            _ = decimal.TryParse(asset.ChangePercent24Hr, out var changePercent24Hr);
            _ = int.TryParse(asset.Rank, out var rank);

            // SaveAssetCommand decide si crear o actualizar y publica AssetSyncedEvent
            await _sender.Send(new SaveAssetCommand(
                asset.Id, asset.Symbol, asset.Name, rank,
                priceUsd, marketCapUsd, volumeUsd24Hr, changePercent24Hr),
                cancellationToken);
        }

        _logger.LogInformation("Sincronización completada: {Count} activos procesados", assets.Count);

        return new SyncAssetsResult(assets.Count, DateTime.UtcNow);
    }
}
