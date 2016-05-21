using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Tests.Helpers
{
    public class TestDbMigrator : System.Data.Entity.Migrations.DbMigrator
    {
        private readonly DbContextInfo _usersContextInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Data.Entity.Migrations.DbMigrator"/> class.
        /// </summary>
        /// <param name="configuration">Configuration to be used for the migration process.</param>
        public TestDbMigrator(DbMigrationsConfiguration configuration)
            : base(configuration)
        {
            _usersContextInfo = configuration.TargetDatabase == null ?
                new DbContextInfo(configuration.ContextType) :
                new DbContextInfo(configuration.ContextType, configuration.TargetDatabase);
            if (_usersContextInfo.IsConstructible)
            {
                using (var context = _usersContextInfo.CreateInstance())
                {
                    DbMigrationContext.Current.DatabaseName = context.Database.Connection.Database;
                }
            }

        }
    }
}
