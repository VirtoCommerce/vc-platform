using System;
using VirtoCommerce.PowerShell.DatabaseSetup;
using System.Reflection;
using System.IO;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Migrations;

namespace VirtoCommerce.PowerShell.AppConfig
{
	public class SqlAppConfigSampleDatabaseInitializer : SqlAppConfigDatabaseInitializer
	{
		protected override void Seed(EFAppConfigRepository context)
		{
			base.Seed(context);
			FillAppConfigScripts(context);
		}

		private void FillAppConfigScripts(EFAppConfigRepository context)
        {
			// fill db with sample data
            RunCommand(context, "Statistic.sql", "AppConfig");
            RunCommand(context, "DisplayTemplateMapping.sql", "AppConfig");
        }
	}
}