using Coincap.Assets.Domain;
using Coincap.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Coincap.API.Repositories.Assets;

// Implementación concreta de IAssetRepository usando EF Core + SQLite
// Vive en Coincap.API porque es el único proyecto con acceso a AppDbContext
public class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _context;

    public AssetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Asset> Items, int Total)> GetAllAsync(
        int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.Assets.AsQueryable();

        // Filtra por nombre o símbolo si se envió un término de búsqueda
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Name.Contains(search) || a.Symbol.Contains(search));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(a => a.Rank)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<Asset?> GetByIdAsync(string id, CancellationToken ct = default)
        => await _context.Assets.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task UpsertAsync(Asset asset, CancellationToken ct = default)
    {
        // Si la entidad no está siendo rastreada por EF Core es nueva — la agregamos
        // Si ya está rastreada (vino de GetByIdAsync) EF Core detecta los cambios automáticamente
        if (_context.Entry(asset).State == EntityState.Detached)
            _context.Assets.Add(asset);

        await _context.SaveChangesAsync(ct);
    }
}
