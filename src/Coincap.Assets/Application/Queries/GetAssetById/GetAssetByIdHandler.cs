using Coincap.Assets.Application.Queries.GetAssets;
using Coincap.Assets.Domain;
using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Queries.GetAssetById;

// Handler que busca un activo por su Id y retorna Result<T> en lugar de lanzar excepción
public class GetAssetByIdHandler : IRequestHandler<GetAssetByIdQuery, Result<AssetResponse>>
{
    private readonly IAssetRepository _repository;

    public GetAssetByIdHandler(IAssetRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AssetResponse>> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var asset = await _repository.GetByIdAsync(request.Id, cancellationToken);

        // Si no existe retorna Failure — el Controller convierte esto en HTTP 404
        if (asset is null)
            return Result<AssetResponse>.Failure($"Asset '{request.Id}' not found");

        return Result<AssetResponse>.Success(new AssetResponse
        {
            Id = asset.Id,
            Symbol = asset.Symbol,
            Name = asset.Name,
            Rank = asset.Rank,
            PriceUsd = asset.PriceUsd,
            MarketCapUsd = asset.MarketCapUsd,
            VolumeUsd24Hr = asset.VolumeUsd24Hr,
            ChangePercent24Hr = asset.ChangePercent24Hr,
            LastSyncAt = asset.LastSyncAt
        });
    }
}
