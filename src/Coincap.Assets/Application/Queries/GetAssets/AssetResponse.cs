namespace Coincap.Assets.Application.Queries.GetAssets;

// DTO que sale hacia el cliente — nunca exponemos la entidad de dominio directamente
// Así si la entidad cambia internamente, la respuesta de la API puede mantenerse igual
public class AssetResponse
{
    public string Id { get; init; } = string.Empty;
    public string Symbol { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Rank { get; init; }
    public decimal PriceUsd { get; init; }
    public decimal MarketCapUsd { get; init; }
    public decimal VolumeUsd24Hr { get; init; }
    public decimal ChangePercent24Hr { get; init; }
    public DateTime LastSyncAt { get; init; }
}
