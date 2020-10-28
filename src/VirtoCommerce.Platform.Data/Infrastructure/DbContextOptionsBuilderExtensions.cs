using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Replace the default EF value generation  for string primary keys from Guid.ToString() with hyphens to Guid.ToString("N")
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder GenerateCompactGuidForKeys(this DbContextOptionsBuilder builder)
        {
            builder.ReplaceService<IValueGeneratorSelector, CustomRelationalValueGeneratorSelector>();
            return builder;
        }


        /// <summary>
        /// Switch to Database Provider according to 'DatabaseProvider' setting in config
        /// And set connection string 'VirtoCommerce' by default
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseDatabaseProviderSwitcher(
            this DbContextOptionsBuilder optionsBuilder,
            IConfiguration configuration)
        {
            var databaseProvider = configuration.GetValue<string>("VirtoCommerce:DatabaseProvider");
            switch (databaseProvider)
            {
                case Core.PlatformConstants.DatabaseProviders.SqlServer:
                default:
                    {
                        optionsBuilder.UseSqlServer(configuration.GetConnectionString("VirtoCommerce"));
                        break;
                    }
                case Core.PlatformConstants.DatabaseProviders.InMemory:
                    {
                        optionsBuilder.UseInMemoryDatabase("VirtoCommerce");
                        break;
                    }
            }
            return optionsBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="configuration"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder SetConnectionName(
            this DbContextOptionsBuilder optionsBuilder,
            IConfiguration configuration,
            string connectionName)
        {
            var databaseProvider = configuration.GetValue<string>("VirtoCommerce:DatabaseProvider");
            switch (databaseProvider)
            {
                case Core.PlatformConstants.DatabaseProviders.SqlServer:
                default:
                    {
                        var extension = optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>();
                        extension.WithConnectionString(configuration.GetConnectionString(connectionName));
                        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
                        break;
                    }
                case Core.PlatformConstants.DatabaseProviders.InMemory:
                    {
                        var extension = optionsBuilder.Options.FindExtension<InMemoryOptionsExtension>();
                        extension = extension.WithStoreName(connectionName);
                        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
                        break;
                    }
            }

            return optionsBuilder;
        }
    }
}
