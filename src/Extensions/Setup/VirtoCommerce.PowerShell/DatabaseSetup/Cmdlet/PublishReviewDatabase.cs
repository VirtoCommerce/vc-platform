using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PowerShell.Reviews;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Data.Reviews.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Review-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishReviewDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFReviewRepository(connection))
			{
				if (sample)
				{
					SafeWriteVerbose("Running sample scripts");
					new SqlReviewSampleDatabaseInitializer().InitializeDatabase(db);
				}
				else
				{
					SafeWriteVerbose("Running minimum scripts");
					new SetupMigrateDatabaseToLatestVersion<EFReviewRepository, Configuration>().InitializeDatabase(db);
				}
			}
		}
	}
}
