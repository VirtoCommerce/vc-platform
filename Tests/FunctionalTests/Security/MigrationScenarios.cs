using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VirtoCommerce.Foundation.Data.Security.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace FunctionalTests.Security
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_security_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Permission"));
            migrator.Update("0");
            Assert.False(TableExists("Permission"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);
            DropDatabase();
        }

        [Fact]
        public void Can_update_security_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Permission"));
            migrator.Update("0");
            Assert.False(TableExists("Permission"));
            DropDatabase();
        }
    }
}
