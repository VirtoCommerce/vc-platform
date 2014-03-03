using VirtoCommerce.Foundation.Marketing.Model;
using System;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.PowerShell.DatabaseSetup;
using VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion;
using System.Reflection;
using System.IO;

namespace VirtoCommerce.PowerShell.Marketing
{
    public class SqlPromotionDatabaseInitializer : SetupDatabaseInitializer<EFMarketingRepository, Configuration>
	{
        protected override void Seed(EFMarketingRepository context)
		{
			base.Seed(context);
            FillPromotionScripts(context);
		}

        private void FillPromotionScripts(EFMarketingRepository context)
        {
            RunCommand(context, "Coupon.sql", "Marketing");
            RunCommand(context, "Promotion.sql", "Marketing");
            RunCommand(context, "PromotionReward.sql", "Marketing");
            //ExecuteCommand(Path.Combine(GetFrameworkDirectory(), "aspnet_regsql.exe"), string.Format("-C \"{0}\" -ed -et -t PromotionReward", context.Database.Connection.ConnectionString));
        }
	}
}