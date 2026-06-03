using Coincap.Assets.Domain;
using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Queries.GetAssets;

// Handler que ejecuta la lógica de GetAssetsQuery
// Solo lee datos — no modifica nada en la base de datos
public class GetAssetsHandler : IRequestHandler<GetAssetsQuery, PagedResult<AssetResponse>>
{
    private readonly IAssetRepository _repository;

    public GetAssetsHandler(IAssetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AssetResponse>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _repository.GetAllAsync(request.Page, request.PageSize, request.Search, cancellationToken);

        // Mapea las entidades de dominio al DTO de respuesta
        var data = items.Select(a => new AssetResponse
        {
            Id = a.Id,
            Symbol = a.Symbol,
            Name = a.Name,
            Rank = a.Rank,
            PriceUsd = a.PriceUsd,
            MarketCapUsd = a.MarketCapUsd,
            VolumeUsd24Hr = a.VolumeUsd24Hr,
            ChangePercent24Hr = a.ChangePercent24Hr,
            LastSyncAt = a.LastSyncAt
        });

        return new PagedResult<AssetResponse>(data, total, request.Page, request.PageSize);
    }
}
