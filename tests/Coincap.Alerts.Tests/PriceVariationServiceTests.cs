using Coincap.Alerts.Application.Services;

namespace Coincap.Alerts.Tests;

public class PriceVariationServiceTests
{
    private readonly PriceVariationService _service = new();
    private const double Threshold = 5.0;

    // Casos NO debe generar alerta

    [Fact]
    public void Calculate_WhenPreviousPriceIsNull_ReturnsNull()
    {
        // Si no hay precio anterior no hay alerta
        var result = _service.Calculate(null, 100m, Threshold);

        Assert.Null(result);
    }

    [Fact]
    public void Calculate_WhenPreviousPriceIsZero_ReturnsNull()
    {
        // Precio cero retornar null
        var result = _service.Calculate(0m, 100m, Threshold);

        Assert.Null(result);
    }

    [Fact]
    public void Calculate_WhenVariationIsBelowThreshold_DoesNotExceed()
    {
        // 4.99% no supera 5%
        var previousPrice = 100m;
        var newPrice = 104.99m;

        var result = _service.Calculate(previousPrice, newPrice, Threshold);

        Assert.NotNull(result);
        Assert.False(result.ExceedsThreshold);
    }

    // Casos Si debe generar alerta

    [Fact]
    public void Calculate_WhenVariationExceedsThreshold_ExceedsIsTrue()
    {
        // 5.01% supera 5%
        var previousPrice = 100m;
        var newPrice = 105.01m;

        var result = _service.Calculate(previousPrice, newPrice, Threshold);

        Assert.NotNull(result);
        Assert.True(result.ExceedsThreshold);
    }

    [Fact]
    public void Calculate_WhenVariationIsExactlyAtThreshold_ExceedsIsTrue()
    {
        // Exactamente 5% — alerta
        var previousPrice = 100m;
        var newPrice = 105m;

        var result = _service.Calculate(previousPrice, newPrice, Threshold);

        Assert.NotNull(result);
        Assert.True(result.ExceedsThreshold);
    }

    // Calculo correcto del porcentaje

    [Fact]
    public void Calculate_ChangePercentIsCalculatedCorrectly()
    {
        // 100 → 110 = 10%
        var result = _service.Calculate(100m, 110m, Threshold);

        Assert.NotNull(result);
        Assert.Equal(10m, result.ChangePercent);
    }

    [Fact]
    public void Calculate_WhenPriceDrops_ChangePercentIsPositive()
    {
        // ChangePercent siempre es positivo
        // 100 → 90 = 10% (no -10%)
        var result = _service.Calculate(100m, 90m, Threshold);

        Assert.NotNull(result);
        Assert.True(result.ChangePercent > 0);
    }
}
