using Coincap.Assets.Domain;
using Coincap.Share;
using Coincap.Share.Events;
using MediatR;

namespace Coincap.Assets.Application.Commands.UpdateAsset;

// Solo actualiza — nunca inserta. Responsabilidad única y clara
public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, Result>
{
    private readonly IAssetRepository _repository;
    private readonly IPublisher _publisher;

    public UpdateAssetHandler(IAssetRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        // Guarda el precio anterior antes de actualizar — lo necesita Alerts para calcular variación
        decimal previousPrice = request.ExistingAsset.PriceUsd;

        request.ExistingAsset.Update(
            request.Rank, request.PriceUsd, request.MarketCapUsd,
            request.VolumeUsd24Hr, request.ChangePercent24Hr);

        await _repository.UpsertAsync(request.ExistingAsset, cancellationToken);

        await _publisher.Publish(new AssetSyncedEvent(
            request.ExistingAsset.Id, request.ExistingAsset.Symbol,
            request.ExistingAsset.Name, previousPrice, request.PriceUsd), cancellationToken);

        return Result.Success();
    }
}
