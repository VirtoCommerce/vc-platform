using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>
    {
        public PlatformDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PlatformDbContext>();

            var databaseProvider = args.First();

            switch (databaseProvider)
            {
                case "PostgreSql":
                    builder.UseNpgsql("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=virtocommerce;", x => x.MigrationsAssembly("VirtoCommerce.Platform.Data.PostgreSql"));
                    break;
                case "MySql":
                    builder.UseMySql("server=localhost;user=root;password=virto;database=VirtoCommerce3;", new MySqlServerVersion(new Version(5, 7)), x => x.MigrationsAssembly("VirtoCommerce.Platform.Data.MySql"));
                    break;
                case "SqlServer":
                    builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");
                    break;
                default:
                    throw new Exception($"Could not find database provider {databaseProvider}. Please specify an argument. Ex: -Args MySql");
            }

            return new PlatformDbContext(builder.Options);
        }
    }
}
