using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoOrderDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishOrderDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            var connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new EFOrderRepository(connection))
            {
                if (sample)
                {
                    SafeWriteVerbose("Running sample scripts");
                    new SqlOrderSampleDatabaseInitializer().InitializeDatabase(db);
                }
                else
                {
                    SafeWriteVerbose("Running minimum scripts");
                    new SqlOrderDatabaseInitializer().InitializeDatabase(db);
                }
            }
        }
    }
}
