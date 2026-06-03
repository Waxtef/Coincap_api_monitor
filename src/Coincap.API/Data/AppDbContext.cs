using Coincap.Alerts.Domain;
using Coincap.Assets.Domain;
using Coincap.PriceHistory.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coincap.API.Data;

// Contexto principal de EF Core — es el puente entre las entidades del dominio y SQLite
// Todos los DbSets (tablas) se agregan aquí a medida que se crean las entidades
public class AppDbContext : DbContext
{
    // Cada DbSet representa una tabla en SQLite
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<PriceSnapshot> PriceSnapshots => Set<PriceSnapshot>();
    public DbSet<PriceAlert> PriceAlerts => Set<PriceAlert>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Aplica automáticamente todas las configuraciones de entidades (IEntityTypeConfiguration)
    // que estén definidas en cualquier módulo del proyecto
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
