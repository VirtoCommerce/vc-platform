using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.PricingModule.Data.Repositories;

namespace VirtoCommerce.PricingModule.Data.Repositories
{
	public class PricingSampleDatabaseInitializer : SetupDatabaseInitializer<PricingRepositoryImpl, VirtoCommerce.PricingModule.Data.Migrations.Configuration>
    {
		protected override void Seed(PricingRepositoryImpl context)
        {
            base.Seed(context);

            foreach (var file in _orderFiles)
            {
               // ExecuteSqlScriptFile(context, file, "Sql");
            }
        }


        private readonly string[] _orderFiles =
		{ 
            "Price.sql",
            "Pricelist.sql",
            "PricelistAssignment.sql"
		};
    }
}
