using FunctionalTests.TestHelpers;
using System.Linq;
using Xunit;
using VirtoCommerce.Foundation.Data.Catalogs.Migrations;
using VirtoCommerce.Foundation.Data.Catalogs;

namespace FunctionalTests.Catalogs
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_catalog_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();
            Assert.True(TableExists("Catalog"));

            using (var context = CreateContext<EFCatalogRepository>())
            {
                Assert.Equal(0, context.TaxCategories.Count());
                Assert.Equal(3, context.Pricelists.Count());
                Assert.Equal(5, context.Packagings.Count());
            }

            // remove all migrations
            migrator.Update("0");
            Assert.False(TableExists("Catalog"));
            var existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }

        [Fact]
        public void Can_update_catalog_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Catalog"));
            migrator.Update("0");
            Assert.False(TableExists("Catalog"));
            DropDatabase();
        }
    }
}
