namespace Coincap.PriceHistory.Application.Queries.GetPriceHistory;

public class PriceSnapshotResponse
{
    public Guid Id { get; init; }
    public string AssetId { get; init; } = string.Empty;
    public string AssetSymbol { get; init; } = string.Empty;
    public decimal PriceUsd { get; init; }
    public DateTime RecordedAt { get; init; }
}
