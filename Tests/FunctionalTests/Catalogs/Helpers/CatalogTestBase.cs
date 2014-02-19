using System;
using System.Data.Entity;
using System.IO;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data;
using VirtoCommerce.Foundation.Data.Catalogs;

namespace FunctionalTests.Catalogs.Helpers
{
    using VirtoCommerce.Foundation.Data.Catalogs.Migrations;
    using VirtoCommerce.PowerShell.DatabaseSetup;

    [JsonSupportBehavior]
	public class TestDSCatalogService : DSCatalogService
	{
		public const string DatabaseName = "CatalogTest";

        protected override EFCatalogRepository CreateRepository()
		{
			return new EFCatalogRepository(DatabaseName, new CatalogEntityFactory());
		}
	}

	public abstract class CatalogTestBase : FunctionalTestBase, IDisposable
	{
		#region Infrastructure/setup
		private TestDataService _Service;

		private readonly object _previousDataDirectory;

		protected CatalogTestBase()
		{
			_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
			AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);
		}

		public override void Init(RepositoryProvider provider)
		{
			if (provider == RepositoryProvider.DataService)
			{
				_Service = new TestDataService(typeof(TestDSCatalogService));
			}

			base.Init(provider);
		}

		public void Dispose()
		{
			try
			{
				// Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
				// the temp location in which they are stored is later cleaned.
				using (var context = new EFCatalogRepository(TestDSCatalogService.DatabaseName))
				{
					context.Database.Delete();
				}

				if (_Service != null)
				{
					_Service.Dispose();
				}
			}
			finally
			{
				AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
			}
		}

		#endregion

		protected ICatalogRepository GetRepository()
		{
			EnsureDatabaseInitialized(() => new EFCatalogRepository(TestDSCatalogService.DatabaseName), () => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFCatalogRepository, Configuration>()));

			if (RepositoryProvider == RepositoryProvider.DataService)
			{
				return new DSCatalogClient(_Service.ServiceUri, new CatalogEntityFactory(), null);
			}
			else
			{
				return new EFCatalogRepository(TestDSCatalogService.DatabaseName);
			}
		}

		protected void RefreshRepository(ref ICatalogRepository client)
		{
			client.Dispose();
			GC.Collect();
			client = GetRepository();
		}

		protected void CreateFullGraphCatalog(ICatalogRepository client, ref Item[] items, string catalogId)
		{
			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogId).WithCategory("category").WithProducts(2);
			var catalog = catalogBuilder.GetCatalog();
			items = catalogBuilder.GetItems();

			client.Add(catalog);

			foreach (var item in items)
			{
				client.Add(item);
			}

			client.UnitOfWork.Commit();
		}
	}
}
