using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Testing.Helpers
{
    public class TestDbMigrator : System.Data.Entity.Migrations.DbMigrator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="System.Data.Entity.Migrations.DbMigrator"/> class.
        /// </summary>
        /// <param name="configuration">Configuration to be used for the migration process.</param>
        public TestDbMigrator(DbMigrationsConfiguration configuration)
            : base(configuration)
        {
            var usersContextInfo = configuration.TargetDatabase == null ?
                new DbContextInfo(configuration.ContextType) :
                new DbContextInfo(configuration.ContextType, configuration.TargetDatabase);

            if (usersContextInfo.IsConstructible)
            {
                using (var context = usersContextInfo.CreateInstance())
                {
                    DbMigrationContext.Current.DatabaseName = context.Database.Connection.Database;
                }
            }
        }
    }
}
