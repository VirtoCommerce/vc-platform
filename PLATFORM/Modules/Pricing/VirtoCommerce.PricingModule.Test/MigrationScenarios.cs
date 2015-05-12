using System.Linq;
using VirtoCommerce.Platform.Tests.Bases;
using VirtoCommerce.PricingModule.Data.Migrations;
using VirtoCommerce.PricingModule.Data.Repositories;
using Xunit;

namespace VirtoCommerce.PricingModule.Test
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        [Trait("Category", "CI")]
        public void Can_create_price_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            //migrator.Update();
            //Assert.True(TableExists("Pricelist"));

            using (var context = CreateContext<PricingRepositoryImpl>())
            {
                new PricingSampleDatabaseInitializer().InitializeDatabase(context);
                Assert.Equal(3, context.Pricelists.Count());
            }

            // remove all migrations
            migrator.Update("0");
            Assert.False(TableExists("Pricelist"));
            var existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }
    }
}
