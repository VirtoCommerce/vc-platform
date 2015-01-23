using FunctionalTests.TestHelpers;
using System.Linq;
using Xunit;
using VirtoCommerce.Foundation.Data.Stores.Migrations;

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
