using System.Linq;

namespace VirtoCommerce.Foundation.Data.Security
{
	public class SqlSecuritySampleDatabaseInitializer : SqlSecurityDatabaseInitializer
	{
		protected override void Seed(EFSecurityRepository context)
		{
			base.Seed(context);
			FillScripts(context);
		}

		private void FillScripts(EFSecurityRepository context)
		{
			ExecuteSqlScriptFile(context, "FillTestAccounts.sql", "Security");

			context.Accounts.First(a => a.AccountId == "1").StoreId = "SampleStore";
		}
	}
}
