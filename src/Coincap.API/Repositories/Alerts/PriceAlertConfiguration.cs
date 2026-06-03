using Coincap.Alerts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coincap.API.Repositories.Alerts;

// Define cómo se mapea PriceAlert a la tabla SQLite
public class PriceAlertConfiguration : IEntityTypeConfiguration<PriceAlert>
{
    public void Configure(EntityTypeBuilder<PriceAlert> builder)
    {
        builder.ToTable("PriceAlerts");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AssetId).HasMaxLength(100).IsRequired();
        builder.Property(a => a.AssetName).HasMaxLength(200).IsRequired();
        builder.Property(a => a.AssetSymbol).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Direction).HasMaxLength(4).IsRequired();
        builder.Property(a => a.PriceBefore).HasPrecision(36, 18);
        builder.Property(a => a.PriceAfter).HasPrecision(36, 18);
        builder.Property(a => a.ChangePercent).HasPrecision(18, 8);

        // Índice en DetectedAt para consultas por fecha y en AssetId para filtrar por moneda
        builder.HasIndex(a => a.DetectedAt);
        builder.HasIndex(a => a.AssetId);
    }
}
