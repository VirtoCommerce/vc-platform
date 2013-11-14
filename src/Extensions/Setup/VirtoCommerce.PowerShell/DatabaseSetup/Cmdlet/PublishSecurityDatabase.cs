using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PowerShell.Security;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Security.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Security-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishSecurityDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFSecurityRepository(connection))
			{
				if (sample)
				{
					SafeWriteVerbose("Running sample scripts");
					new SqlSecuritySampleDatabaseInitializer().InitializeDatabase(db);
				}
				else
				{
					SafeWriteVerbose("Running minimum scripts");
					new SqlSecurityDatabaseInitializer().InitializeDatabase(db);
				}
			}
		}
	}
}
