using System.Linq;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.Foundation.Data.Security
{
	public class SqlSecuritySampleDatabaseInitializer : SqlSecurityDatabaseInitializer
	{
		protected override void Seed(EFSecurityRepository context)
		{
			base.Seed(context);

			CreateTestAccount(context);
			UpdateAdminAccount(context);
		}

		private static void CreateTestAccount(EFSecurityRepository context)
		{
			context.Add(new Account
			{
				AccountId = "2",
				MemberId = "2",
				UserName = "test",
				RegisterType = (int)RegisterType.Administrator,
				AccountState = (int)AccountState.Approved,
				StoreId = "SampleStore",
			});

			context.UnitOfWork.Commit();
		}

		private static void UpdateAdminAccount(EFSecurityRepository context)
		{
			context.Accounts.First(a => a.AccountId == "1").StoreId = "SampleStore";
			context.UnitOfWork.Commit();
		}
	}
}
