using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Review-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishReviewDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, reduced, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFReviewRepository(connection))
			{
				SqlReviewDatabaseInitializer initializer;

				if (sample)
				{
					SafeWriteVerbose("Running sample scripts");
					initializer = new SqlReviewSampleDatabaseInitializer();
				}
				else
				{
					SafeWriteVerbose("Running minimum scripts");
					initializer = new SqlReviewDatabaseInitializer();
				}

				initializer.InitializeDatabase(db);
			}
		}
	}
}
