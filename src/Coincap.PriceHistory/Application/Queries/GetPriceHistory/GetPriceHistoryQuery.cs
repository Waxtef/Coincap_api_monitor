using MediatR;

namespace Coincap.PriceHistory.Application.Queries.GetPriceHistory;

// Query para obtener el historial de precios de una moneda
// ffecha opcionales
public record GetPriceHistoryQuery(string AssetId, DateTime? From, DateTime? To)
    : IRequest<IEnumerable<PriceSnapshotResponse>>;
