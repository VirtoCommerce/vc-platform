using System;
using VirtoCommerce.PowerShell.DatabaseSetup;
using System.Reflection;
using System.IO;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Importing.Migrations;

namespace VirtoCommerce.PowerShell.Import
{
    public class SqlImportDatabaseInitializer : SetupDatabaseInitializer<EFImportingRepository, Configuration>
	{
		protected override void Seed(EFImportingRepository context)
		{
			base.Seed(context);
            FillImportScripts(context);
		}

		private void FillImportScripts(EFImportingRepository context)
        {
			//RunCommand(context, "MappingItem.sql", "Import");
			//RunCommand(context, "ImportJob.sql", "Import");
        }
	}
}