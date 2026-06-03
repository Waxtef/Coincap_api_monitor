namespace Coincap.Share.Events;

// Evento publicado por ExternalAPI cada vez que sincroniza un activo con CoinCap
// PriceHistory y Alerts lo escuchan para hacer su trabajo sin conocerse entre sí
public class AssetSyncedEvent : DomainEvent
{
    public string AssetId { get; }
    public string Symbol { get; }
    public string Name { get; }

    // Precio anterior guardado en BD — null si es la primera vez que se sincroniza
    public decimal? PreviousPriceUsd { get; }

    public decimal NewPriceUsd { get; }

    public AssetSyncedEvent(
        string assetId,
        string symbol,
        string name,
        decimal? previousPriceUsd,
        decimal newPriceUsd)
    {
        AssetId = assetId;
        Symbol = symbol;
        Name = name;
        PreviousPriceUsd = previousPriceUsd;
        NewPriceUsd = newPriceUsd;
    }
}
