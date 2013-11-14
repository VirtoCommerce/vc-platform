using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PowerShell.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-AppConfig-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishAppConfigDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFAppConfigRepository(connection))
			{
				if (sample)
				{
					SafeWriteVerbose("Running sample scripts");
					new SqlAppConfigSampleDatabaseInitializer().InitializeDatabase(db);
				}
				else
				{
					SafeWriteVerbose("Running minimum scripts");
					new SqlAppConfigDatabaseInitializer().InitializeDatabase(db);
				}
			}
		}
	}
}
