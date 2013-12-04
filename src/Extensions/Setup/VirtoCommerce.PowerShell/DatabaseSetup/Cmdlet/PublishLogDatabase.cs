using System;
using System.Management.Automation;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.LogMigrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Log-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class PublishLogDatabase : DatabaseCommand
    {
        public override void Publish(string dbconnection, string data, bool sample)
        {
            base.Publish(dbconnection, data, sample);
            string connection = dbconnection;
            SafeWriteDebug("ConnectionString: " + connection);

            using (var db = new OperationLogContext(connection))
            {
                new SetupMigrateDatabaseToLatestVersion<OperationLogContext, Configuration>().InitializeDatabase(db);
            }
        }
    }
}
