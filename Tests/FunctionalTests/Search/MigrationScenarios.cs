using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VirtoCommerce.Foundation.Data.Search.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace FunctionalTests.Search
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_search_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("BuildSetting"));
            migrator.Update("0");
            Assert.False(TableExists("BuildSetting"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);
            DropDatabase();
        }

        [Fact]
        public void Can_update_search_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("BuildSetting"));
            migrator.Update("0");
            Assert.False(TableExists("BuildSetting"));
            DropDatabase();
        }
    }
}
