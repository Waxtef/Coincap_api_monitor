namespace Coincap.Alerts.Domain;

// Contrato para guardar y consultar alertas de variación de precio
public interface IPriceAlertRepository
{
    Task AddAsync(PriceAlert alert, CancellationToken ct = default);

    // Retorna alertas con filtro opcional por moneda específica
    Task<IEnumerable<PriceAlert>> GetAllAsync(string? assetId, CancellationToken ct = default);
}
