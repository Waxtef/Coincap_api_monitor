using Coincap.PriceHistory.Domain;
using MediatR;

namespace Coincap.PriceHistory.Application.Queries.GetPriceHistory;

// Consulta el historial de precios y mapea las respuestas
public class GetPriceHistoryHandler : IRequestHandler<GetPriceHistoryQuery, IEnumerable<PriceSnapshotResponse>>
{
    private readonly IPriceSnapshotRepository _repository;

    public GetPriceHistoryHandler(IPriceSnapshotRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PriceSnapshotResponse>> Handle(
        GetPriceHistoryQuery request, CancellationToken cancellationToken)
    {
        var snapshots = await _repository.GetByAssetIdAsync(
            request.AssetId, request.From, request.To, cancellationToken);

        return snapshots.Select(s => new PriceSnapshotResponse
        {
            Id = s.Id,
            AssetId = s.AssetId,
            AssetSymbol = s.AssetSymbol,
            PriceUsd = s.PriceUsd,
            RecordedAt = s.RecordedAt
        });
    }
}
