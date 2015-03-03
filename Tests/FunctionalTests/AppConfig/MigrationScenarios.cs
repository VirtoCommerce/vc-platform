using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Data.AppConfig.Migrations;
using Xunit;

namespace FunctionalTests.AppConfig
{
	public class MigrationScenarios : MigrationsTestBase
	{
		[Fact]
		public void Can_create_marketing_promotion_new_database()
		{
			DropDatabase();

			var migrator = CreateMigrator<Configuration>();
			migrator.Update();

			Assert.True(TableExists("Setting"));
			migrator.Update("0");
            Assert.False(TableExists("Setting"));
			bool existTables = Info.Tables.Any();
			Assert.False(existTables);
			DropDatabase();
		}

		[Fact]
		public void Can_update_marketing_promotion_empty_database()
		{
			ResetDatabase();

			var migrator = CreateMigrator<Configuration>();
			migrator.Update();

            Assert.True(TableExists("Setting"));
			migrator.Update("0");
            Assert.False(TableExists("Setting"));
			DropDatabase();
		}
	}
}
