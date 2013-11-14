using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PowerShell.Stores;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Data.Stores.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Store-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishStoreDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFStoreRepository(connection))
			{
				if (sample)
				{
					SafeWriteVerbose("Running sample scripts");
					new SqlStoreSampleDatabaseInitializer().InitializeDatabase(db);
				}
				else
				{
					SafeWriteVerbose("Running minimum scripts");
					new SetupMigrateDatabaseToLatestVersion<EFStoreRepository, Configuration>().InitializeDatabase(db);
				}
			}
		}
	}
}
