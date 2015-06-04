using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoCatalogDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishCatalogDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new EFCatalogRepository(connection))
            {
                SqlCatalogDatabaseInitializer initializer;

                if (!string.IsNullOrEmpty(data) && sample)
                {
                    if (reduced)
                    {
                        SafeWriteVerbose("Running reduced sample scripts");
                        initializer = new SqlCatalogReducedSampleDatabaseInitializer { DataDirectoryPath = data };
                    }
                    else
                    {
                        SafeWriteVerbose("Running sample scripts");
                        initializer = new SqlCatalogSampleDatabaseInitializer { DataDirectoryPath = data };
                    }

                }
                else
                {
                    SafeWriteVerbose("Running minimum scripts");
                    initializer = new SqlCatalogDatabaseInitializer();
                }

                initializer.InitializeDatabase(db);
            }
        }
    }
}
