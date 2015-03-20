using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VirtoCommerce.Foundation.Data.Reviews.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace FunctionalTests.Reviews
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_reviews_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Review"));
            migrator.Update("0");
            Assert.False(TableExists("Review"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }

        [Fact]
        public void Can_update_reviews_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Review"));
            migrator.Update("0");
            Assert.False(TableExists("Review"));

            DropDatabase();
        }
    }
}
