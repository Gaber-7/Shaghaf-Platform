using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shaghaf.Infrastructure.Persistence;

/// <summary>
/// Used by the EF Core tooling (migrations) without booting the API host.
/// </summary>
public class ShaghafDbContextFactory : IDesignTimeDbContextFactory<ShaghafDbContext>
{
    public ShaghafDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("SHAGHAF_CONNECTION_STRING")
            ?? "Server=.;Database=ShaghafDB;Trusted_Connection=true;TrustServerCertificate=true;";

        var options = new DbContextOptionsBuilder<ShaghafDbContext>()
            .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(ShaghafDbContext).Assembly.FullName))
            .Options;

        return new ShaghafDbContext(options);
    }
}
