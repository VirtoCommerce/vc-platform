using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static void MigrateIfNotApplied(this DatabaseFacade databaseFacade, string targetMigration)
        {
            var timeout = databaseFacade.GetService<IOptions<DataOptions>>().Value.MigrationsTimeout
                ?? TimeSpan.FromSeconds(databaseFacade.GetDbConnection().ConnectionTimeout);

            databaseFacade.SetCommandTimeout(timeout);

            var platformMigrator = databaseFacade.GetService<IMigrator>();
            var appliedMigrations = databaseFacade.GetAppliedMigrations();
            if (!appliedMigrations.Any(x => x.EqualsInvariant(targetMigration)))
            {
                platformMigrator.Migrate(targetMigration);
            }
        }
    }
}
