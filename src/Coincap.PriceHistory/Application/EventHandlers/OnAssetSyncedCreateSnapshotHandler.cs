using Coincap.PriceHistory.Domain;
using Coincap.Share.Events;
using MediatR;

namespace Coincap.PriceHistory.Application.EventHandlers;

// guarda el precio en ese momento para ponerlo en el historial
public class OnAssetSyncedCreateSnapshotHandler : INotificationHandler<AssetSyncedEvent>
{
    private readonly IPriceSnapshotRepository _repository;

    public OnAssetSyncedCreateSnapshotHandler(IPriceSnapshotRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AssetSyncedEvent notification, CancellationToken cancellationToken)
    {
        var snapshot = new PriceSnapshot(
            notification.AssetId,
            notification.Symbol,
            notification.NewPriceUsd,
            notification.OccurredAt);

        await _repository.AddAsync(snapshot, cancellationToken);
    }
}
