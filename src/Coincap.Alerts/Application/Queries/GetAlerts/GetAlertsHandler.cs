using Coincap.Alerts.Domain;
using MediatR;

namespace Coincap.Alerts.Application.Queries.GetAlerts;

// Consulta las alertas y las mapea a DTOs de respuesta
public class GetAlertsHandler : IRequestHandler<GetAlertsQuery, IEnumerable<AlertResponse>>
{
    private readonly IPriceAlertRepository _repository;

    public GetAlertsHandler(IPriceAlertRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AlertResponse>> Handle(
        GetAlertsQuery request, CancellationToken cancellationToken)
    {
        var alerts = await _repository.GetAllAsync(request.AssetId, cancellationToken);

        return alerts.Select(a => new AlertResponse
        {
            Id = a.Id,
            AssetId = a.AssetId,
            AssetName = a.AssetName,
            AssetSymbol = a.AssetSymbol,
            PriceBefore = a.PriceBefore,
            PriceAfter = a.PriceAfter,
            ChangePercent = a.ChangePercent,
            Direction = a.Direction,
            DetectedAt = a.DetectedAt
        });
    }
}
