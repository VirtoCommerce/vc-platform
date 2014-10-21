using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.PowerShell.Marketing;
using VirtoCommerce.Foundation.Data.Marketing;
using m = VirtoCommerce.Foundation.Data.Marketing.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Marketing-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishMarketingDatabase : DatabaseCommand
	{
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, reduced, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFMarketingRepository(connection))
			{
                if (sample)
                {
                    new SqlPromotionDatabaseInitializer().InitializeDatabase(db);
                }
                else
                {
                    new SetupMigrateDatabaseToLatestVersion<EFMarketingRepository, m.Promotion.Configuration>().InitializeDatabase(db);
                }
			}

			using (var db = new EFDynamicContentRepository(connection))
			{
				if (sample)
				{
					new SqlDynamicContentSampleDatabaseInitializer().InitializeDatabase(db);
				}
				else
				{
                    new SqlDynamicContentDatabaseInitializer().InitializeDatabase(db);
				}
			}
		}
	}
}