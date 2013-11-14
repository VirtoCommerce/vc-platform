using System;
using VirtoCommerce.PowerShell.DatabaseSetup;
using System.Reflection;
using System.IO;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Migrations;

namespace VirtoCommerce.PowerShell.AppConfig
{
	public class SqlAppConfigDatabaseInitializer : SetupDatabaseInitializer<EFAppConfigRepository, Configuration>
	{
		protected override void Seed(EFAppConfigRepository context)
		{
			base.Seed(context);
			FillAppConfigScripts(context);
		}

		private void FillAppConfigScripts(EFAppConfigRepository context)
		{
			// fill db with data
			RunCommand(context, "Setting.sql", "AppConfig");
			RunCommand(context, "SettingValue.sql", "AppConfig");
			RunCommand(context, "SystemJob.sql", "AppConfig");
            RunCommand(context, "JobParameter.sql", "AppConfig");
			RunCommand(context, "EmailTemplate.sql", "AppConfig");
			RunCommand(context, "EmailTemplateLanguage.sql", "AppConfig");
			RunCommand(context, "Localization.sql", "AppConfig");
		}
	}
}