using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Importing
{
	public class SqlImportDatabaseInitializer : SetupDatabaseInitializer<EFImportingRepository, Migrations.Configuration>
	{
	}
}
