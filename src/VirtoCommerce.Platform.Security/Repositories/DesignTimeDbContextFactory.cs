using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
    {
        public SecurityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SecurityDbContext>();

            var databaseProvider = args.First();

            switch (databaseProvider)
            {
                case "PostgreSql":
                    builder.UseNpgsql("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=virtocommerce;", x => x.MigrationsAssembly("VirtoCommerce.Platform.Security.PostgreSql"));
                    break;
                case "MySql":
                    builder.UseMySql("server=localhost;user=root;password=virto;database=VirtoCommerce3;", new MySqlServerVersion(new Version(5, 7)), x => x.MigrationsAssembly("VirtoCommerce.Platform.Security.MySql"));
                    break;
                case "SqlServer":
                    builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");
                    break;
                default:
                    throw new Exception($"Could not find database provider {databaseProvider}. Please specify an argument. Ex: -Args MySql");
            }

            builder.UseOpenIddict();

            return new SecurityDbContext(builder.Options);
        }
    }
}
