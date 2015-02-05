using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.PowerShell.DatabaseSetup;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Security.Migrations;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.PowerShell.Security
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
			RunCommand(context, "FillRequiredCaseProperties.sql", "Security");
			RunCommand(context, "FillTestAccounts.sql", "Security");

			context.Accounts.First(a => a.AccountId == "1").StoreId = "SampleStore";
        }
    }
}
