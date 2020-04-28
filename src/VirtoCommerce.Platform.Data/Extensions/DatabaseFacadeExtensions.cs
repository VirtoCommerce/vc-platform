using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static void MigrateIfNotApplied(this DatabaseFacade databaseFacade, string targetMigration)
        {
            var connectionTimeout = databaseFacade.GetDbConnection().ConnectionTimeout;
            databaseFacade.SetCommandTimeout(connectionTimeout);

            var platformMigrator = databaseFacade.GetService<IMigrator>();
            var appliedMigrations = databaseFacade.GetAppliedMigrations();
            if (!appliedMigrations.Any(x => x.EqualsInvariant(targetMigration)))
            {
                platformMigrator.Migrate(targetMigration);
            }
        }
    }
}
