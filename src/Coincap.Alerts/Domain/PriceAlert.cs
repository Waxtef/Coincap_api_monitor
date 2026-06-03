using Coincap.Share;

namespace Coincap.Alerts.Domain;

// Representa una alerta generada cuando un activo supera el umbral de variación configurado
public class PriceAlert : Entity<Guid>
{
    public string AssetId { get; private set; } = string.Empty;
    public string AssetName { get; private set; } = string.Empty;
    public string AssetSymbol { get; private set; } = string.Empty;
    public decimal PriceBefore { get; private set; }
    public decimal PriceAfter { get; private set; }

    // Porcentaje de variación absoluto — siempre positivo, la dirección indica si subió o bajó
    public decimal ChangePercent { get; private set; }

    // UP si el precio subió, DOWN si bajó
    public string Direction { get; private set; } = string.Empty;

    public DateTime DetectedAt { get; private set; }

    // Constructor vacío requerido por EF Core
    private PriceAlert() { }

    public PriceAlert(string assetId, string assetName, string assetSymbol,
        decimal priceBefore, decimal priceAfter, decimal changePercent, string direction)
        : base(Guid.NewGuid())
    {
        AssetId = assetId;
        AssetName = assetName;
        AssetSymbol = assetSymbol;
        PriceBefore = priceBefore;
        PriceAfter = priceAfter;
        ChangePercent = changePercent;
        Direction = direction;
        DetectedAt = DateTime.UtcNow;
    }
}
