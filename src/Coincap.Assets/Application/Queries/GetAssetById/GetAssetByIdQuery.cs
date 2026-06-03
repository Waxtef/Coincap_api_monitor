using Coincap.Assets.Application.Queries.GetAssets;
using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Queries.GetAssetById;

// Query que representa la intención de obtener el detalle de un activo específico por su Id
public record GetAssetByIdQuery(string Id) : IRequest<Result<AssetResponse>>;
