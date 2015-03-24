using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoSearchDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishSearchDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            SafeWriteDebug("ConnectionString: " + dbconnection);

            using (var db = new EFSearchRepository(dbconnection))
            {
                new SearchDatabaseInitializer().InitializeDatabase(db);
            }
        }
    }
}
