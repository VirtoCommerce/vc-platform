using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static void MigrateIfNotApplied(this DatabaseFacade databaseFacade, string targetMigration)
        {
            var configuration = databaseFacade.GetService<IConfiguration>();
            var commandTimeout = configuration.GetValue<int?>("VirtoCommerce:CommandTimeout");
            databaseFacade.SetCommandTimeout(commandTimeout);

            var platformMigrator = databaseFacade.GetService<IMigrator>();
            var appliedMigrations = databaseFacade.GetAppliedMigrations();
            if (!appliedMigrations.Any(x => x.EqualsInvariant(targetMigration)))
            {
                platformMigrator.Migrate(targetMigration);
            }
        }

        public static void MigrateAndConfigCommandTimeout(this DatabaseFacade databaseFacade)
        {
            var configuration = databaseFacade.GetService<IConfiguration>();
            var commandTimeout = configuration.GetValue<int?>("VirtoCommerce:CommandTimeout");
            databaseFacade.SetCommandTimeout(commandTimeout);
            databaseFacade.Migrate();
        }
    }
}
