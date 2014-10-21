using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.PowerShell.Customers;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Customers.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Customer-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishCustomerDatabase : DatabaseCommand
	{
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, reduced, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFCustomerRepository(connection))
			{
				if (sample)
				{
					SafeWriteVerbose("Running sample scripts");
					new SqlCustomerDatabaseInitializer().InitializeDatabase(db);
				}
				else
				{
					SafeWriteVerbose("Running minimum scripts");
					new SetupMigrateDatabaseToLatestVersion<EFCustomerRepository, Configuration>().InitializeDatabase(db);
				}
			}
		}
	}
}
