using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.Platform.Data.PostgreSql
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the context to use PostgreSql.
        /// </summary>
        public static DbContextOptionsBuilder UsePostgreSqlDatabase(this DbContextOptionsBuilder builder, string connectionString) =>
            builder.UseNpgsql(connectionString, db => db
                .MigrationsAssembly(typeof(PostgreSqlDbContextFactory).Assembly.GetName().Name));
    }
}
