using Coincap.API.Data;
using Coincap.PriceHistory.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coincap.API.Repositories.PriceHistory;

// Implementación concreta de IPriceSnapshotRepository usando EF Core + SQLite
public class PriceSnapshotRepository : IPriceSnapshotRepository
{
    private readonly AppDbContext _context;

    public PriceSnapshotRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PriceSnapshot snapshot, CancellationToken ct = default)
    {
        await _context.PriceSnapshots.AddAsync(snapshot, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<PriceSnapshot>> GetByAssetIdAsync(
        string assetId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var query = _context.PriceSnapshots
            .Where(s => s.AssetId == assetId)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(s => s.RecordedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(s => s.RecordedAt <= to.Value);

        // Ordenado cronológicamente para que el cliente vea la evolución del precio
        return await query.OrderBy(s => s.RecordedAt).ToListAsync(ct);
    }
}
