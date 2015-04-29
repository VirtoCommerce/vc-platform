using VirtoCommerce.OrderModule.Data.Repositories;

namespace VirtoCommerce.OrderModule.Web.SampleData
{
    public class OrderSampleDatabaseInitializer : OrderDatabaseInitializer
    {
        protected override void Seed(OrderRepositoryImpl context)
        {
            base.Seed(context);

            foreach (var file in _orderFiles)
            {
                ExecuteSqlScriptFile(context, file, "SampleData");
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
