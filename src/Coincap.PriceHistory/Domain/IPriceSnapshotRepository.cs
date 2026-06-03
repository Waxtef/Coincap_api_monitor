namespace Coincap.PriceHistory.Domain;

// Contrato para guardar y consultar snapshots de precios
public interface IPriceSnapshotRepository
{
    // Agrega un nuevo snapshot al historial
    Task AddAsync(PriceSnapshot snapshot, CancellationToken ct = default);

    // Retorna el historial de precios de un activo con filtro opcional de fechas
    Task<IEnumerable<PriceSnapshot>> GetByAssetIdAsync(string assetId, DateTime? from, DateTime? to, CancellationToken ct = default);
}
