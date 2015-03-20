using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VirtoCommerce.Foundation.Data.Marketing.Migrations.Content;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace FunctionalTests.Marketing.Content
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_marketing_content_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("DynamicContentPlace"));
            migrator.Update("0");
            Assert.False(TableExists("DynamicContentPlace"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);
            DropDatabase();
        }

        [Fact]
        public void Can_update_marketing_content_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("DynamicContentPlace"));
            migrator.Update("0");
            Assert.False(TableExists("DynamicContentPlace"));
            DropDatabase();
        }
    }
}
