using System;
using System.IO;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.StoreModule.Data.Repositories;

namespace VirtoCommerce.StoreModule.Web.SampleData
{
	public class SqlStoreDatabaseInitializer : SetupDatabaseInitializer<StoreRepositoryImpl, VirtoCommerce.StoreModule.Data.Migrations.Configuration>
	{
	}
}
