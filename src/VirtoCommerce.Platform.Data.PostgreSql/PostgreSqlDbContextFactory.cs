using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Data.PostgreSql
{
    public class PostgreSqlDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>,
        IDesignTimeDbContextFactory<SecurityDbContext>
    {
        PlatformDbContext IDesignTimeDbContextFactory<PlatformDbContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PlatformDbContext>();
            var connectionString = args.Any() ? args[0] : "User ID = postgres; Password = password; Host = localhost; Port = 5432; Database = virtocommerce3;";

            builder.UseNpgsql(
                connectionString,
                db => db.MigrationsAssembly(typeof(PostgreSqlDbContextFactory).Assembly.GetName().Name));

            return new PlatformDbContext(builder.Options);
        }

        SecurityDbContext IDesignTimeDbContextFactory<SecurityDbContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SecurityDbContext>();
            var connectionString = args.Any() ? args[0] : "User ID = postgres; Password = password; Host = localhost; Port = 5432; Database = virtocommerce3;";

            builder.UseNpgsql(
                connectionString,
                db => db.MigrationsAssembly(typeof(PostgreSqlDbContextFactory).Assembly.GetName().Name));

            builder.UseOpenIddict();

            return new SecurityDbContext(builder.Options);
        }

    }
}
