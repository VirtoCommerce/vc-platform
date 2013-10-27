using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Search.Migrations;

namespace VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet
{
	[CLSCompliant(false)]
	[Cmdlet(VerbsData.Publish, "Virto-Search-Database", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
	public class PublishSearchDatabase : DatabaseCommand
	{
		public override void Publish(string dbconnection, string data, bool sample)
		{
			base.Publish(dbconnection, data, sample);
			string connection = dbconnection;
			SafeWriteDebug("ConnectionString: " + connection);

			using (var db = new EFSearchRepository(connection))
			{
				new SetupMigrateDatabaseToLatestVersion<EFSearchRepository, Configuration>().InitializeDatabase(db);
			}
		}
	}
}
