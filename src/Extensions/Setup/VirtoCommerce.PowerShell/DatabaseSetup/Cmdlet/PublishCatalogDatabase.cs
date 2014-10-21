using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.PowerShell.Catalogs;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Catalogs.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "Virto-Catalog-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishCatalogDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new EFCatalogRepository(connection))
            {
                if (!string.IsNullOrEmpty(data) && sample)
                {
                    if (reduced)
                    {
                        SafeWriteVerbose("Running reduced sample scripts");
                        new SqlCatalogReducedSampleDatabaseInitializer(data).InitializeDatabase(db);
                    }
                    else
                    {
                        SafeWriteVerbose("Running sample scripts");
                        new SqlCatalogSampleDatabaseInitializer(data).InitializeDatabase(db);
                    }
                   
                }
                else
                {
                    SafeWriteVerbose("Running minimum scripts");
                    new SetupMigrateDatabaseToLatestVersion<EFCatalogRepository, Configuration>().InitializeDatabase(db);
                }
            }
        }
    }
}