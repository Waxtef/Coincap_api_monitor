namespace Coincap.Alerts.Application.Services;

// Calcula si la variación de precio entre dos syncs supera el umbral configurado
public class PriceVariationService
{
    // Calcula la variación y determina si supera el umbral
    // Retorna null si no hay precio anterior (primer sync del activo — no se puede comparar)
    public VariationResult? Calculate(decimal? previousPrice, decimal newPrice, double thresholdPercent)
    {
        if (previousPrice is null or 0)
            return null;

        var changePercent = Math.Abs((newPrice - previousPrice.Value) / previousPrice.Value * 100);
        var direction = newPrice >= previousPrice.Value ? "UP" : "DOWN";

        return new VariationResult(
            ChangePercent: changePercent,
            Direction: direction,
            ExceedsThreshold: changePercent >= (decimal)thresholdPercent
        );
    }
}

// Resultado del cálculo de variación
public record VariationResult(decimal ChangePercent, string Direction, bool ExceedsThreshold);
