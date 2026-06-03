using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Coincap.API.Data;

// Usada SOLO por EF Core Tools en tiempo de diseño (Add-Migration, Update-Database)
// Evita que las herramientas tengan que ejecutar todo el Program.cs para crear el DbContext
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=coincap.db");
        return new AppDbContext(optionsBuilder.Options);
    }
}
