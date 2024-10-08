using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

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
        return optionsBuilder.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            mySqlDbContextOptionsBuilder =>
            {
                mySqlDbContextOptionsBuilder.MigrationsAssembly(migrationsAssemblyMarkerType.Assembly.GetName().Name);
                mySqlOptionsAction?.Invoke(mySqlDbContextOptionsBuilder, configuration);
            });
    }
}
