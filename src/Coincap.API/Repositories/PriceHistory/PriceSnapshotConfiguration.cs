using Coincap.PriceHistory.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coincap.API.Repositories.PriceHistory;

// Define cómo se mapea PriceSnapshot a la tabla SQLite
public class PriceSnapshotConfiguration : IEntityTypeConfiguration<PriceSnapshot>
{
    public void Configure(EntityTypeBuilder<PriceSnapshot> builder)
    {
        builder.ToTable("PriceSnapshots");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.AssetId).HasMaxLength(100).IsRequired();
        builder.Property(s => s.AssetSymbol).HasMaxLength(20).IsRequired();
        builder.Property(s => s.PriceUsd).HasPrecision(36, 18);

        // Índice compuesto para optimizar consultas de historial por activo y fecha
        builder.HasIndex(s => new { s.AssetId, s.RecordedAt });
    }
}
