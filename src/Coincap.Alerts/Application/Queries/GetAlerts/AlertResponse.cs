namespace Coincap.Alerts.Application.Queries.GetAlerts;

// DTO que sale al cliente con la información de la alerta
public class AlertResponse
{
    public Guid Id { get; init; }
    public string AssetId { get; init; } = string.Empty;
    public string AssetName { get; init; } = string.Empty;
    public string AssetSymbol { get; init; } = string.Empty;
    public decimal PriceBefore { get; init; }
    public decimal PriceAfter { get; init; }
    public decimal ChangePercent { get; init; }
    public string Direction { get; init; } = string.Empty;
    public DateTime DetectedAt { get; init; }
}
