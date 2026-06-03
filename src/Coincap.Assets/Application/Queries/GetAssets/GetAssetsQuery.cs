using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Queries.GetAssets;

// Query que representa la intención de obtener el listado paginado de activos
// record es inmutable — una Query nunca debe cambiar sus valores después de crearse
public record GetAssetsQuery(int Page, int PageSize, string? Search) : IRequest<PagedResult<AssetResponse>>;
