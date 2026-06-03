using Coincap.Alerts.Domain;
using Coincap.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Coincap.API.Repositories.Alerts;

// Implementación concreta de IPriceAlertRepository usando EF Core + SQLite
public class PriceAlertRepository : IPriceAlertRepository
{
    private readonly AppDbContext _context;

    public PriceAlertRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PriceAlert alert, CancellationToken ct = default)
    {
        await _context.PriceAlerts.AddAsync(alert, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<PriceAlert>> GetAllAsync(
        string? assetId, CancellationToken ct = default)
    {
        var query = _context.PriceAlerts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(assetId))
            query = query.Where(a => a.AssetId == assetId);

        // Las más recientes primero
        return await query.OrderByDescending(a => a.DetectedAt).ToListAsync(ct);
    }
}
