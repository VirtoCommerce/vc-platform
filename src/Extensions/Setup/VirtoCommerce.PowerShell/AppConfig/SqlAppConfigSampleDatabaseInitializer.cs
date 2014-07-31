using VirtoCommerce.Foundation.Data.AppConfig;

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
            RunCommand(context, "DisplayTemplateMapping.sql", "AppConfig");
            RunCommand(context, "SeoUrlKeyword.sql", "AppConfig");
        }
    }
}