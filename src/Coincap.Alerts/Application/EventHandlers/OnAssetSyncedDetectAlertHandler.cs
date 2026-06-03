using Coincap.Alerts.Application.Services;
using Coincap.Alerts.Domain;
using Coincap.Share.Events;
using MediatR;
using Microsoft.Extensions.Options;

namespace Coincap.Alerts.Application.EventHandlers;

// Escucha AssetSyncedEvent y genera una alerta si la variación supera el umbral configurado
public class OnAssetSyncedDetectAlertHandler : INotificationHandler<AssetSyncedEvent>
{
    private readonly IPriceAlertRepository _repository;
    private readonly PriceVariationService _variationService;
    private readonly double _thresholdPercent;

    public OnAssetSyncedDetectAlertHandler(
        IPriceAlertRepository repository,
        PriceVariationService variationService,
        IOptions<AlertOptions> options)
    {
        _repository = repository;
        _variationService = variationService;
        _thresholdPercent = options.Value.AlertThresholdPercent;
    }

    public async Task Handle(AssetSyncedEvent notification, CancellationToken cancellationToken)
    {
        var result = _variationService.Calculate(
            notification.PreviousPriceUsd,
            notification.NewPriceUsd,
            _thresholdPercent);

        // Sin resultado = primer sync del activo, no hay precio anterior para comparar
        if (result is null || !result.ExceedsThreshold)
            return;

        var alert = new PriceAlert(
            notification.AssetId,
            notification.Name,
            notification.Symbol,
            notification.PreviousPriceUsd!.Value,
            notification.NewPriceUsd,
            result.ChangePercent,
            result.Direction);

        await _repository.AddAsync(alert, cancellationToken);
    }
}
