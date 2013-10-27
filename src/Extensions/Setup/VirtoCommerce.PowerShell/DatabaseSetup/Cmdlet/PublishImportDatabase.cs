using System;
using System.Management.Automation;
using VirtoCommerce.PowerShell.Import;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Importing.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Import-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishImportDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFImportingRepository(connection))
			{
				if (sample)
                {
					SafeWriteVerbose("Running sample scripts");
                    new SqlImportDatabaseInitializer().InitializeDatabase(db);
                }
                else
				{
					SafeWriteVerbose("Running minimum scripts");
					new SetupMigrateDatabaseToLatestVersion<EFImportingRepository, Configuration>().InitializeDatabase(db);
				}
			}
		}
	}
}
