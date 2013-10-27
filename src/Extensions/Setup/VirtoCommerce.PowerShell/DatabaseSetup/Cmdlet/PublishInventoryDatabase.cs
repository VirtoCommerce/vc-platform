using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Data.Inventories;
using VirtoCommerce.Foundation.Data.Inventories.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Inventory-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishInventoryDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);


			using (var db = new EFInventoryRepository(connection))
			{
				new SetupMigrateDatabaseToLatestVersion<EFInventoryRepository, Configuration>().InitializeDatabase(db);
			}
		}
	}
}
