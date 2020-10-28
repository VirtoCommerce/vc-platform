using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static void MigrateIfNotApplied(this DatabaseFacade databaseFacade, string targetMigration)
        {
            if (databaseFacade.IsRelationalDatabase())
            {
                databaseFacade.SetCommandTimeout();

                var platformMigrator = databaseFacade.GetService<IMigrator>();
                var appliedMigrations = databaseFacade.GetAppliedMigrations();
                if (!appliedMigrations.Any(x => x.EqualsInvariant(targetMigration)))
                {
                    platformMigrator.Migrate(targetMigration);
                }
            }
        }

        public static void MigrateIfRelationalDatabase(this DatabaseFacade databaseFacade)
        {
            if (databaseFacade.IsRelationalDatabase())
            {
                databaseFacade.SetCommandTimeout();
                databaseFacade.Migrate();
            }
        }
            

        public static bool IsRelationalDatabase(this DatabaseFacade databaseFacade)
        {
            return databaseFacade.GetService<IDatabaseCreator>() is RelationalDatabaseCreator;
        }

        /// <summary>
        /// Set Command Timeout according to 'CommandTimeout' setting in config
        /// </summary>
        /// <param name="databaseFacade"></param>
        public static void SetCommandTimeout(this DatabaseFacade databaseFacade)
        {
            if (databaseFacade.IsRelationalDatabase())
            {
                var configuration = databaseFacade.GetService<IConfiguration>();
                var commandTimeout = configuration.GetValue<int?>("VirtoCommerce:CommandTimeout");
                databaseFacade.SetCommandTimeout(commandTimeout);
            }
        }
    }
}
