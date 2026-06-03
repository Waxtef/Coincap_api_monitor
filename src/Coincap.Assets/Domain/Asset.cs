using Coincap.Share;

namespace Coincap.Assets.Domain;

// Aggregate Root del módulo Assets — representa una criptomoneda guardada en la BD
// Hereda de Entity<string> porque CoinCap usa IDs de texto como "bitcoin" o "ethereum"
public class Asset : Entity<string>
{
    public string Symbol { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int Rank { get; private set; }
    public decimal PriceUsd { get; private set; }
    public decimal MarketCapUsd { get; private set; }
    public decimal VolumeUsd24Hr { get; private set; }

    // Variación de precio en las últimas 24 horas — puede ser negativa si bajó
    public decimal ChangePercent24Hr { get; private set; }
    public DateTime LastSyncAt { get; private set; }

    // Constructor vacío requerido por EF Core
    private Asset() { }

    public Asset(string id, string symbol, string name, int rank,
        decimal priceUsd, decimal marketCapUsd, decimal volumeUsd24Hr, decimal changePercent24Hr)
        : base(id)
    {
        Symbol = symbol;
        Name = name;
        Rank = rank;
        PriceUsd = priceUsd;
        MarketCapUsd = marketCapUsd;
        VolumeUsd24Hr = volumeUsd24Hr;
        ChangePercent24Hr = changePercent24Hr;
        LastSyncAt = DateTime.UtcNow;
    }

    // Actualiza los datos cuando llega un nuevo sync — guarda el precio anterior antes de cambiar
    public void Update(int rank, decimal priceUsd, decimal marketCapUsd,
        decimal volumeUsd24Hr, decimal changePercent24Hr)
    {
        Rank = rank;
        PriceUsd = priceUsd;
        MarketCapUsd = marketCapUsd;
        VolumeUsd24Hr = volumeUsd24Hr;
        ChangePercent24Hr = changePercent24Hr;
        LastSyncAt = DateTime.UtcNow;
    }
}
