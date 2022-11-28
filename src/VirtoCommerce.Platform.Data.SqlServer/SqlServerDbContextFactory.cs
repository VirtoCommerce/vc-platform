using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Data.SqlServer
{
    public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>,
        IDesignTimeDbContextFactory<SecurityDbContext>
    {
        PlatformDbContext IDesignTimeDbContextFactory<PlatformDbContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PlatformDbContext>();
            var connectionString = args.Any() ? args[0] : "Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30";

            builder.UseSqlServer(
                connectionString,
                db => db.MigrationsAssembly(typeof(SqlServerDbContextFactory).Assembly.GetName().Name));

            return new PlatformDbContext(builder.Options);
        }

        SecurityDbContext IDesignTimeDbContextFactory<SecurityDbContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SecurityDbContext>();
            var connectionString = args.Any() ? args[0] : "Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30";

            builder.UseSqlServer(
                connectionString,
                db => db.MigrationsAssembly(typeof(SqlServerDbContextFactory).Assembly.GetName().Name));

            builder.UseOpenIddict();

            return new SecurityDbContext(builder.Options);
        }
    }
}
