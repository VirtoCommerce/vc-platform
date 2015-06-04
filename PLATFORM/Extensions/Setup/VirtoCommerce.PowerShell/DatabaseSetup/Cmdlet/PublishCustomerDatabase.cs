using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Publish, "VirtoCustomerDatabase", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishCustomerDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample, bool reduced, string strategy = SqlDbConfiguration.SqlAzureExecutionStrategy)
        {
            base.Publish(dbconnection, data, sample, reduced, strategy);
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new EFCustomerRepository(connection))
            {
                SqlCustomerDatabaseInitializer initializer;

                if (sample)
                {
                    SafeWriteVerbose("Running sample scripts");
                    initializer = new SqlCustomerSampleDatabaseInitializer();
                }
                else
                {
                    SafeWriteVerbose("Running minimum scripts");
                    initializer = new SqlCustomerDatabaseInitializer();
                }

                initializer.InitializeDatabase(db);
            }
        }
    }
}
