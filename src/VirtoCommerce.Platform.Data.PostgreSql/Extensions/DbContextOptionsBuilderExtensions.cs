using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using VirtoCommerce.Platform.Core;

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
        // Enhance connection string with performance and reliability settings
        var postgreSqlOptions = configuration.GetSection("PostgreSql").Get<PostgreSqlOptions>() ?? new PostgreSqlOptions();
        connectionString = postgreSqlOptions.EnhanceConnectionString(connectionString);

        return optionsBuilder.UseNpgsql(connectionString,
            npgsqlDbContextOptionsBuilder =>
            {
                npgsqlDbContextOptionsBuilder.MigrationsAssembly(migrationsAssemblyMarkerType.Assembly.GetName().Name);

                // Set command timeout
                npgsqlDbContextOptionsBuilder.CommandTimeout(postgreSqlOptions.CommandTimeout);

                // Enable retry on failure for transient errors (Azure PostgreSQL specific)
                if (postgreSqlOptions.EnableRetryOnFailure)
                {
                    npgsqlDbContextOptionsBuilder.EnableRetryOnFailure(
                        maxRetryCount: postgreSqlOptions.MaxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(postgreSqlOptions.MaxRetryDelaySeconds),
                        errorCodesToAdd: null);
                }

                npgsqlOptionsAction?.Invoke(npgsqlDbContextOptionsBuilder, configuration);
            });
    }
}
