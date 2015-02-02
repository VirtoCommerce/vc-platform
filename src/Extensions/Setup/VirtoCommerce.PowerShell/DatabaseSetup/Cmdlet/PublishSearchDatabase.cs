using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Search.Migrations;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Search-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishSearchDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, reduced, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFSearchRepository(connection))
			{
				new SetupDatabaseInitializer<EFSearchRepository, Configuration>().InitializeDatabase(db);
			}
		}
	}
}
