using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VirtoCommerce.Foundation.Data.Stores.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet;

namespace FunctionalTests.Stores
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_stores_new_database()
        {
            DropDatabase();
            
            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Store"));
            migrator.Update("0");
            Assert.False(TableExists("Store"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);
            DropDatabase();
        }

        [Fact]
        public void Can_update_stores_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Store"));
            migrator.Update("0");
            Assert.False(TableExists("Store"));
            DropDatabase();
        }
    }
}
