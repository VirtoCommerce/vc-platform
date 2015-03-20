using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VirtoCommerce.Foundation.Data.Orders.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace FunctionalTests.Orders
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_order_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("OrderGroup"));
            migrator.Update("0");
            Assert.False(TableExists("OrderGroup"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);
            DropDatabase();
        }

        [Fact]
        public void Can_update_order_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("OrderGroup"));
            migrator.Update("0");
            Assert.False(TableExists("OrderGroup"));
            DropDatabase();
        }
    }
}
