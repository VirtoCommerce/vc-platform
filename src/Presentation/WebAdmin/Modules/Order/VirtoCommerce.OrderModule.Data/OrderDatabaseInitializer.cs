using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.OrderModule.Data.Repositories;

namespace VirtoCommerce.OrderModule.Data
{
	public class OrderDatabaseInitializer : SetupDatabaseInitializer<OrderRepositoryImpl, Migrations.Configuration>
	{
	}
}
