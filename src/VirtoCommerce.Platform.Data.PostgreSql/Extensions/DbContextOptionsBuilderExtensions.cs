using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace VirtoCommerce.Platform.Data.PostgreSql.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the context to use PostgreSql.
    /// </summary>
    public static DbContextOptionsBuilder UsePostgreSqlDatabase(
        this DbContextOptionsBuilder optionsBuilder,
        string? connectionString,
        Type migrationsAssemblyMarkerType,
        IConfiguration configuration,
        Action<NpgsqlDbContextOptionsBuilder, IConfiguration>? npgsqlOptionsAction = null)
    {
        return optionsBuilder.UseNpgsql(connectionString,
            npgsqlDbContextOptionsBuilder =>
            {
                npgsqlDbContextOptionsBuilder.MigrationsAssembly(migrationsAssemblyMarkerType.Assembly.GetName().Name);
                npgsqlOptionsAction?.Invoke(npgsqlDbContextOptionsBuilder, configuration);
            });
    }
}
