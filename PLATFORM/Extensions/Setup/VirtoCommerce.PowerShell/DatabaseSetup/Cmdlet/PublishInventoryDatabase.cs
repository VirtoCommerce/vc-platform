using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Inventories;
using VirtoCommerce.Foundation.Data.Inventories.Migrations;
using VirtoCommerce.Foundation.Frameworks;

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
				new SetupDatabaseInitializer<EFInventoryRepository, Configuration>().InitializeDatabase(db);
			}
		}
	}
}
