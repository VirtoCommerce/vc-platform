using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Web.SampleData
{
    public class OrderDatabaseInitializer : SetupDatabaseInitializer<OrderRepositoryImpl, VirtoCommerce.OrderModule.Data.Migrations.Configuration>
    {
    }
}
