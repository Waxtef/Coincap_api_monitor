namespace Coincap.Assets.Domain;

// Contrato que define las operaciones disponibles sobre activos en la base de datos
// La implementación concreta vive en Infrastructure — el dominio no sabe nada de EF Core o SQLite
public interface IAssetRepository
{
    // Retorna todos los activos con soporte de paginación y búsqueda por nombre o símbolo
    Task<(IEnumerable<Asset> Items, int Total)> GetAllAsync(int page, int pageSize, string? search, CancellationToken ct = default);

    // Busca un activo por su Id de CoinCap
    Task<Asset?> GetByIdAsync(string id, CancellationToken ct = default);

    // Inserta un activo nuevo o actualiza uno existente según su Id
    Task UpsertAsync(Asset asset, CancellationToken ct = default);
}
