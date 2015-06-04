using System;
using System.IO;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Catalogs
{
	public class SqlCatalogDatabaseInitializer : SetupDatabaseInitializer<EFCatalogRepository, Migrations.Configuration>
	{
	}
}
