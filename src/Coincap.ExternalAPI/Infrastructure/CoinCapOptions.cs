namespace Coincap.ExternalAPI.Infrastructure;

// Mapea la sección "CoinCap" del appsettings.json a una clase tipada
// Se inyecta via IOptions<CoinCapOptions> en los servicios que la necesitan
public class CoinCapOptions
{
    public const string Section = "CoinCap";

    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public int SyncIntervalMinutes { get; set; } = 5;
    public int RetryCount { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 30;
    public double AlertThresholdPercent { get; set; } = 5.0;
    public int AssetLimit { get; set; } = 100;
}
