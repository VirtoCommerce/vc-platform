using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Search.Migrations;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Search-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishSearchDatabase : DatabaseCommand
	{
        public override void Publish(string dbconnection, string data, bool sample, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFSearchRepository(connection))
			{
				new SetupMigrateDatabaseToLatestVersion<EFSearchRepository, Configuration>().InitializeDatabase(db);
			}
		}
	}
}
