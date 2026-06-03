using Coincap.Assets.Application.Commands.CreateAsset;
using Coincap.Assets.Application.Commands.UpdateAsset;
using Coincap.Assets.Domain;
using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Commands.SaveAsset;

// Orquestador — verifica si el activo existe y delega a CreateAsset o UpdateAsset
// No contiene lógica de inserción ni actualización directa
public class SaveAssetHandler : IRequestHandler<SaveAssetCommand, Result>
{
    private readonly IAssetRepository _repository;
    private readonly ISender _sender;

    public SaveAssetHandler(IAssetRepository repository, ISender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task<Result> Handle(SaveAssetCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (existing is null)
        {
            // Activo nuevo — delega a CreateAssetCommand
            return await _sender.Send(new CreateAssetCommand(
                request.Id, request.Symbol, request.Name, request.Rank,
                request.PriceUsd, request.MarketCapUsd, request.VolumeUsd24Hr,
                request.ChangePercent24Hr), cancellationToken);
        }

        // Activo existente — delega a UpdateAssetCommand con el precio anterior
        return await _sender.Send(new UpdateAssetCommand(
            existing, request.Rank, request.PriceUsd, request.MarketCapUsd,
            request.VolumeUsd24Hr, request.ChangePercent24Hr), cancellationToken);
    }
}
