using VirtoCommerce.OrderModule.Data.Repositories;

namespace VirtoCommerce.OrderModule.Data.Orders
{
    public class OrderSampleDatabaseInitializer : OrderDatabaseInitializer
    {
        protected override void Seed(OrderRepositoryImpl context)
        {
            base.Seed(context);

            foreach (var file in _orderFiles)
            {
                ExecuteSqlScriptFile(context, file, "Orders");
            }
        }


        private readonly string[] _orderFiles =
		{ 
            "order_CustomerOrder.sql",
            "order_PaymentIn.sql",
            "order_Shipment.sql",
            "order_Address.sql",
            "order_LineItem.sql"
		};
    }
}
