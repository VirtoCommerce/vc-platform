using System.Linq;
using VirtoCommerce.Content.Data;
using VirtoCommerce.Content.Data.Migrations;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Platform.Tests.Bases;
using Xunit;

namespace VirtoCommerce.Content.Tests
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        [Trait("Category", "CI")]
        public void CanCreateContentNewDatabase()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();

            using (var context = CreateContext<DatabaseContentRepositoryImpl>())
            {
                new SqlContentDatabaseInitializer().InitializeDatabase(context);
                Assert.True(context.Pages.Any());
            }

            // remove all migrations
            migrator.Update("0");
            Assert.False(TableExists("ContentPage"));
            var existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }
    }
}
