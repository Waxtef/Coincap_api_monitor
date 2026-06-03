using MediatR;

namespace Coincap.Alerts.Application.Queries.GetAlerts;

// Query para obtener alertas con filtro opcional por moneda
public record GetAlertsQuery(string? AssetId)
    : IRequest<IEnumerable<AlertResponse>>;
