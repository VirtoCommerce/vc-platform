using System.Linq;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Migrations;
using VirtoCommerce.PowerShell.DatabaseSetup;

namespace VirtoCommerce.PowerShell.AppConfig
{
	public class SqlAppConfigDatabaseInitializer : SetupDatabaseInitializer<EFAppConfigRepository, Configuration>
	{
		public string Scope { private get; set; }

		protected override void Seed(EFAppConfigRepository context)
		{
			base.Seed(context);
			FillAppConfigScripts(context);
			UpdateScopeParameterValue(context);
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

		private void UpdateScopeParameterValue(EFAppConfigRepository context)
		{
			if (!string.IsNullOrEmpty(Scope))
			{
				var scopeParameters = context.SystemJobs.Where(x => x.JobParameters.Any(p => p.Name == "scope"))
											 .SelectMany(x => x.JobParameters)
											 .Where(x => x.Name == "scope")
											 .ToList();
				scopeParameters.ForEach(x => x.Value = Scope);
				context.SaveChanges();
			}
		}
	}
}