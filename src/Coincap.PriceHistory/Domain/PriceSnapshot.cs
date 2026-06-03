using Coincap.Share;

namespace Coincap.PriceHistory.Domain;

// Representa una lista de los precios de un currency en un momento especifico
// Cada sync genera un PriceSnapshot por currency
public class PriceSnapshot : Entity<Guid>
{
    public string AssetId { get; private set; } = string.Empty;
    public string AssetSymbol { get; private set; } = string.Empty;
    public decimal PriceUsd { get; private set; }

    // Fecha exacta en que guardo el precio
    public DateTime RecordedAt { get; private set; }

    // Constructor vacio requerido por EF Core
    private PriceSnapshot() { }

    public PriceSnapshot(string assetId, string assetSymbol, decimal priceUsd, DateTime recordedAt)
        : base(Guid.NewGuid())
    {
        AssetId = assetId;
        AssetSymbol = assetSymbol;
        PriceUsd = priceUsd;
        RecordedAt = recordedAt;
    }
}
