namespace VirtoCommerce.Foundation.Data.Marketing
{
	public class SqlPromotionSampleDatabaseInitializer : SqlPromotionDatabaseInitializer
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
