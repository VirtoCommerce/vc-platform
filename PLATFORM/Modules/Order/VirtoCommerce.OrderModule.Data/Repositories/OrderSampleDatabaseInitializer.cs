using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
	public class OrderSampleDatabaseInitializer : SetupDatabaseInitializer<OrderRepositoryImpl, VirtoCommerce.OrderModule.Data.Migrations.Configuration>
    {
        protected override void Seed(OrderRepositoryImpl context)
        {
            base.Seed(context);

            foreach (var file in _orderFiles)
            {
               // ExecuteSqlScriptFile(context, file, "Sql");
            }
        }


        private readonly string[] _orderFiles =
		{ 
			"CustomerOrder.sql",
			"OrderPaymentIn.sql",
			"OrderShipment.sql",
			"OrderAddress.sql",
			"OrderLineItem.sql"
		};
    }
}
