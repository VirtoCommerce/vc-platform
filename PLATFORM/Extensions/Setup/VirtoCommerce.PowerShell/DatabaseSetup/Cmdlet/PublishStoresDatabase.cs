using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoStoreDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishStoreDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            var connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            try
            {
                using (var db = new EFStoreRepository(connection))
                {
                    SqlStoreDatabaseInitializer initializer;

                    if (sample)
                    {
                        SafeWriteVerbose(string.Format("Running {0}sample scripts", reduced ? "reduced " : ""));
                        initializer = new SqlStoreSampleDatabaseInitializer(reduced);
                    }
                    else
                    {
                        SafeWriteVerbose("Running minimum scripts");
                        initializer = new SqlStoreDatabaseInitializer();
                    }

                    initializer.InitializeDatabase(db);
                }
            }
            catch (Exception ex)
            {
                SafeThrowError(ex);
            }
        }
    }
}
