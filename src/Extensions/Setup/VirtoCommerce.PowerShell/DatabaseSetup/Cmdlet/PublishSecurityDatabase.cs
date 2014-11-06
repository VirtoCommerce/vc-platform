using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.PowerShell.Security;
using VirtoCommerce.Foundation.Data.Security;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Security-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishSecurityDatabase : DatabaseCommand
	{
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
		{
			base.Publish(dbconnection, data, sample, reduced, strategy);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

            try
            {
                using (var db = new EFSecurityRepository(connection))
                {
                    if (sample)
                    {
                        SafeWriteVerbose("Running sample scripts");
                        new SqlSecuritySampleDatabaseInitializer().InitializeDatabase(db);
                    }
                    else
                    {
                        SafeWriteVerbose("Running minimum scripts");
                        new SqlSecurityDatabaseInitializer().InitializeDatabase(db);
                    }
                }
            }
            catch (Exception ex)
            {
                SafeThrowError(ex);
            }
		}
	}
}
