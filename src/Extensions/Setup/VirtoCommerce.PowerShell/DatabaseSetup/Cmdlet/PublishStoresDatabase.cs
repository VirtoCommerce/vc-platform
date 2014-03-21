using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.PowerShell.Stores;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Data.Stores.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Store-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishStoreDatabase : DatabaseCommand
	{
        public override void Publish(string dbconnection, string data, bool sample, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, strategy);
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
