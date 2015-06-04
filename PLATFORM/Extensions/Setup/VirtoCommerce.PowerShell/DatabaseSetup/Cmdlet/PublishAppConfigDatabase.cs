using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoAppConfigDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishAppConfigDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            PublishWithScope(dbconnection, data, sample, reduced, null);
        }

        public void PublishWithScope(string dbconnection, string data, bool sample, bool reduced, string scope)
        {
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new EFAppConfigRepository(connection))
            {
                SqlAppConfigDatabaseInitializer initializer;

                if (sample)
                {
                    if (reduced)
                    {
                        SafeWriteVerbose("Running reduced sample scripts");
                        initializer = new SqlAppConfigReducedSampleDatabaseInitializer();
                    }
                    else
                    {
                        SafeWriteVerbose("Running sample scripts");
                        initializer = new SqlAppConfigSampleDatabaseInitializer();
                    }

                }
                else
                {
                    SafeWriteVerbose("Running minimum scripts");
                    initializer = new SqlAppConfigDatabaseInitializer();
                }

                initializer.Scope = scope;
                initializer.InitializeDatabase(db);
            }
        }
    }
}
