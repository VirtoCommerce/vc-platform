using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Customers
{
	public class SqlCustomerDatabaseInitializer : SetupDatabaseInitializer<EFCustomerRepository, Migrations.Configuration>
	{
	}
}
