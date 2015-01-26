using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Stores
{
	public class SqlStoreDatabaseInitializer : SetupDatabaseInitializer<EFStoreRepository, Migrations.Configuration>
	{
	}
}
