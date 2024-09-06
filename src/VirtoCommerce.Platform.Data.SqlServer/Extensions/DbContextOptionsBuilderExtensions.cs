using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Data.SqlServer.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the context to use SqlServer.
    /// </summary>
    public static DbContextOptionsBuilder UseSqlServerDatabase(
        this DbContextOptionsBuilder optionsBuilder,
        string? connectionString,
        Type migrationsAssemblyMarkerType,
        IConfiguration configuration,
        Action<SqlServerDbContextOptionsBuilder, IConfiguration>? sqlServerOptionsAction = null)
    {
        return optionsBuilder.UseSqlServer(connectionString,
            sqlServerOptionsBuilder =>
            {
                var compatibilityLevel = configuration.GetValue<int?>("SqlServer:CompatibilityLevel", null);
                if (compatibilityLevel != null)
                {
                    sqlServerOptionsBuilder.UseCompatibilityLevel(compatibilityLevel.Value);
                }

                sqlServerOptionsBuilder.MigrationsAssembly(migrationsAssemblyMarkerType.Assembly.GetName().Name);
                sqlServerOptionsAction?.Invoke(sqlServerOptionsBuilder, configuration);
            });
    }
}
