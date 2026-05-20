using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace VirtoCommerce.Platform.Data.MySql.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the context to use MySql.
    /// </summary>
    public static DbContextOptionsBuilder UseMySqlDatabase(
        this DbContextOptionsBuilder optionsBuilder,
        string? connectionString,
        Type migrationsAssemblyMarkerType,
        IConfiguration configuration,
        Action<MySqlDbContextOptionsBuilder, IConfiguration>? mySqlOptionsAction = null)
    {
        // EF Core's optimistic-concurrency check needs "matched rows" semantics.
        // MySqlConnector defaults UseAffectedRows=true (returns "changed rows"),
        // which makes EF Core throw DbUpdateConcurrencyException for any UPDATE
        // that matches a row but doesn't change column values.
        var fixedConnectionString = NormalizeConnectionString(connectionString);

        return optionsBuilder.UseMySql(fixedConnectionString,
            ServerVersion.AutoDetect(fixedConnectionString),
            mySqlDbContextOptionsBuilder =>
            {
                mySqlDbContextOptionsBuilder.MigrationsAssembly(migrationsAssemblyMarkerType.Assembly.GetName().Name);
                mySqlOptionsAction?.Invoke(mySqlDbContextOptionsBuilder, configuration);
            })
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NoEntityTypeConfigurationsWarning));
    }

    internal static string NormalizeConnectionString(string? connectionString)
    {
        return new MySqlConnectionStringBuilder(connectionString ?? string.Empty)
        {
            UseAffectedRows = false,
            AllowUserVariables = true,
        }.ConnectionString;
    }
}
