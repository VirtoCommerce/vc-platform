using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.OrderModule.Data.Repositories;

namespace VirtoCommerce.OrderModule.Data.Orders
{
    public class OrderDatabaseInitializer : SetupDatabaseInitializer<OrderRepositoryImpl, Migrations.Configuration>
    {
    }
}
