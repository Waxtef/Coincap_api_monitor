using Coincap.Assets.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coincap.API.Repositories.Assets;

// Define cómo se mapea la entidad Asset a la tabla en SQLite
// AppDbContext la aplica automáticamente via ApplyConfigurationsFromAssembly
public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(100);

        builder.Property(a => a.Symbol).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Name).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Rank).IsRequired();

        // 18 decimales para manejar criptomonedas con valores muy pequeños como SHIB (0.000005)
        builder.Property(a => a.PriceUsd).HasPrecision(36, 18);
        builder.Property(a => a.MarketCapUsd).HasPrecision(36, 18);
        builder.Property(a => a.VolumeUsd24Hr).HasPrecision(36, 18);
        builder.Property(a => a.ChangePercent24Hr).HasPrecision(18, 8);
    }
}
