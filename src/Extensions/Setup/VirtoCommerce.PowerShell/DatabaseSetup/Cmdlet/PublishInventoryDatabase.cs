using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Data.Inventories;
using VirtoCommerce.Foundation.Data.Inventories.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Inventory-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishInventoryDatabase : DatabaseCommand
	{
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, reduced, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);


			using (var db = new EFInventoryRepository(connection))
			{
				new SetupMigrateDatabaseToLatestVersion<EFInventoryRepository, Configuration>().InitializeDatabase(db);
			}
		}
	}
}
