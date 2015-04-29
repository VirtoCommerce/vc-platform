using System;
using System.IO;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Web.SampleData
{
	public class SqlCatalogDatabaseInitializer : SetupDatabaseInitializer<CatalogRepositoryImpl, VirtoCommerce.CatalogModule.Data.Migrations.Configuration>
	{
	}
}
