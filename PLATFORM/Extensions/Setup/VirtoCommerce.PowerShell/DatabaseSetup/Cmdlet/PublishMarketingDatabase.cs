using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Frameworks;

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
				SqlPromotionDatabaseInitializer initializer;

				if (sample)
				{
					initializer = new SqlPromotionSampleDatabaseInitializer();
				}
				else
				{
					initializer = new SqlPromotionDatabaseInitializer();
				}

				initializer.InitializeDatabase(db);
			}

			using (var db = new EFDynamicContentRepository(connection))
			{
				SqlDynamicContentDatabaseInitializer initializer;

				if (sample)
				{
					initializer = new SqlDynamicContentSampleDatabaseInitializer();
				}
				else
				{
					initializer = new SqlDynamicContentDatabaseInitializer();
				}

				initializer.InitializeDatabase(db);
			}
		}
	}
}
