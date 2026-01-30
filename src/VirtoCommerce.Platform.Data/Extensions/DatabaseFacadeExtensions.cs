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
            var commandTimeout = databaseFacade.GetCommandTimeout();
            if (commandTimeout is null || (commandTimeout >= 0 && commandTimeout < connectionTimeout))
            {
                databaseFacade.SetCommandTimeout(connectionTimeout);
            }

            var platformMigrator = databaseFacade.GetService<IMigrator>();
            var appliedMigrations = databaseFacade.GetAppliedMigrations();
            if (!appliedMigrations.ContainsIgnoreCase(targetMigration))
            {
                platformMigrator.Migrate(targetMigration);
            }
        }
    }
}
