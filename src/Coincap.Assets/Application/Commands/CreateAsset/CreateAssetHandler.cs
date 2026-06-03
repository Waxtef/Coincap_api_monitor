using Coincap.Assets.Domain;
using Coincap.Share;
using Coincap.Share.Events;
using MediatR;

namespace Coincap.Assets.Application.Commands.CreateAsset;

// Solo inserta — nunca actualiza. Responsabilidad única y clara
public class CreateAssetHandler : IRequestHandler<CreateAssetCommand, Result>
{
    private readonly IAssetRepository _repository;
    private readonly IPublisher _publisher;

    public CreateAssetHandler(IAssetRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = new Asset(
            request.Id, request.Symbol, request.Name, request.Rank,
            request.PriceUsd, request.MarketCapUsd, request.VolumeUsd24Hr,
            request.ChangePercent24Hr);

        await _repository.UpsertAsync(asset, cancellationToken);

        // PreviousPriceUsd es null porque es un activo nuevo — Alerts no generará alerta
        await _publisher.Publish(new AssetSyncedEvent(
            request.Id, request.Symbol, request.Name, null, request.PriceUsd), cancellationToken);

        return Result.Success();
    }
}
