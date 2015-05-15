using System.Linq;
using VirtoCommerce.MarketingModule.Data.Migrations;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Tests.Bases;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        [Trait("Category", "CI")]
        public void Can_create_marketing_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();

            using (var context = CreateContext<MarketingRepositoryImpl>())
            {
                new SetupDatabaseInitializer<MarketingRepositoryImpl,Configuration>().InitializeDatabase(context);
                Assert.Equal(0, context.Promotions.Count());
            }

            // remove all migrations
            migrator.Update("0");
            Assert.False(TableExists("Promotion"));
            var existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }
    }
}
