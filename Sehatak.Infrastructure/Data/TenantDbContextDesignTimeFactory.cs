using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sehatak.Infrastructure.Data;

public class TenantDbContextDesignTimeFactory
        : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            "Server=localhost;Database=sehatak_design;User=root;Password=;";

        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 0, 30))
        );

        return new TenantDbContext(optionsBuilder.Options);
    }
}
