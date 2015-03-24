using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.LogMigrations;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoLogDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishLogDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new OperationLogContext(connection))
            {
                new SetupDatabaseInitializer<OperationLogContext, Configuration>().InitializeDatabase(db);
            }
        }
    }
}
