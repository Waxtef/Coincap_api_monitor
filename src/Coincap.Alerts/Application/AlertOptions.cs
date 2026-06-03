namespace Coincap.Alerts.Application;

// Mapea la configuración de alertas desde appsettings.json
public class AlertOptions
{
    public const string Section = "CoinCap";

    // Umbral de variación — si el precio cambia más de este % se genera una alerta
    public double AlertThresholdPercent { get; set; } = 5.0;
}
