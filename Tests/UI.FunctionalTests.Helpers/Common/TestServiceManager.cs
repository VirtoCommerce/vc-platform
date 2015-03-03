using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.PowerShell.DatabaseSetup;


// Using TestServiceManager in UI tests
//
// If services that you will use don't added to manager yet, then you need to do following steps.
// 1. Add TestDSxxxxxService into TestDSService.cs.
// 2. Add pseudonym of service in ServiceNameEnum
// 3. Add to GetDbContext method return of DbContext by just added pseudonym.
// 4. Add to GetService method return of TestDataService by just added pseudonym.
//
// Now you are ready to write UI test
// 1. Create class. The class should be derived from FunctionalUITestBase.
// 2. Override DefService() method of FunctionalUITestBase class. For example
//
//		public override void DefService()
//      {
//			//It means we will use two service in the test.
//			ServManager.AddService(ServiceNameEnum.AppConfig);
//			ServManager.AddService(ServiceNameEnum.Catalog);
//		}
//
// 3. Use method GetUri() to get Service Uri by pseudonym.
//
// Examples:
//
// Example of resolving Repository factory in test:
//
//  	var priceListRepositoryFactory = new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(ServManager.GetUri(ServiceNameEnum.Catalog));
//
// Example of resolving VmFactory factory in test:
//
//  	IViewModelsFactory vmFactory = new TestCatalogViewModelFactory(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
//
// Example of TestDSxxxxxService class:
//
//		[JsonSupportBehavior]
//		public class TestDSCatalogService : DSCatalogService
//		{
//			public const string DatabaseName = "CatalogTest";
//
//			protected override EFCatalogRepository CreateDataSource()
//			{
//				return new EFCatalogRepository(DatabaseName, new CatalogEntityFactory());
//			}
//		}



namespace UI.FunctionalTests.Helpers.Common
{
	/// <summary>
	/// Enum of service pseudonyms 
	/// Using in test for work with services.
	/// Methods GetDbContext, GetService has to manage all the pseudonyms )
	/// </summary>
	public enum ServiceNameEnum
	{
		AppConfig,
		Catalog,
		DynamicContent,
		Store,
		Order,
		Marketing
	}

	public class TestServiceManager
	{

		#region Setings of the menager

		private DbContext GetDbContext(ServiceNameEnum service)
		{

			switch (service)
			{
				case ServiceNameEnum.AppConfig:
					return new EFAppConfigRepository(TestDSAppConfigService.DatabaseName, new AppConfigEntityFactory());
				case ServiceNameEnum.Catalog:
					return new EFCatalogRepository(TestDSCatalogService.DatabaseName, new CatalogEntityFactory());
				case ServiceNameEnum.DynamicContent:
					return new EFDynamicContentRepository(TestDSDynamicContentService.DatabaseName, new DynamicContentEntityFactory());
				case ServiceNameEnum.Store:
					return new EFStoreRepository(TestDSStoreService.DatabaseName, new StoreEntityFactory());
				case ServiceNameEnum.Order:
					return new EFOrderRepository(TestDSOrderService.DatabaseName, new OrderEntityFactory());
				case ServiceNameEnum.Marketing:
					return new EFMarketingRepository(TestDSMarketingService.DatabaseName, new MarketingEntityFactory());
			}
			return null;
		}

		private void Initialize(ServiceNameEnum service)
		{
			switch (service)
			{
				case ServiceNameEnum.AppConfig:
					Database.SetInitializer(
						new SetupMigrateDatabaseToLatestVersion
							<EFAppConfigRepository, VirtoCommerce.Foundation.Data.AppConfig.Migrations.Configuration>());
					break;
				case ServiceNameEnum.Catalog:
					Database.SetInitializer(
						new SetupMigrateDatabaseToLatestVersion
							<EFCatalogRepository, VirtoCommerce.Foundation.Data.Catalogs.Migrations.Configuration>());
					break;
				case ServiceNameEnum.DynamicContent:
										Database.SetInitializer(
						new SetupMigrateDatabaseToLatestVersion
							<EFDynamicContentRepository, VirtoCommerce.Foundation.Data.Marketing.Migrations.Content.Configuration>());
					break;
				case ServiceNameEnum.Store:
										Database.SetInitializer(
						new SetupMigrateDatabaseToLatestVersion
							<EFStoreRepository, VirtoCommerce.Foundation.Data.Stores.Migrations.Configuration>());
					break;
				case ServiceNameEnum.Order:
										Database.SetInitializer(
						new SetupMigrateDatabaseToLatestVersion
							<EFOrderRepository, VirtoCommerce.Foundation.Data.Orders.Migrations.Configuration>());
					break;
				case ServiceNameEnum.Marketing:
										Database.SetInitializer(
						new SetupMigrateDatabaseToLatestVersion
							<EFMarketingRepository, VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion.Configuration>());
					break;
			}
		}

		private TestDataService GetService(ServiceNameEnum service)
		{
			switch (service)
			{
				case ServiceNameEnum.AppConfig:
					return new TestDataService(typeof(TestDSAppConfigService));
				case ServiceNameEnum.Catalog:
					return new TestDataService(typeof(TestDSCatalogService));
				case ServiceNameEnum.DynamicContent:
					return new TestDataService(typeof(TestDSDynamicContentService));
				case ServiceNameEnum.Store:
					return new TestDataService(typeof(TestDSStoreService));
				case ServiceNameEnum.Order:
					return new TestDataService(typeof(TestDSOrderService));
				case ServiceNameEnum.Marketing:
					return new TestDataService(typeof(TestDSMarketingService));
			}
			return null;
		}
		
		#endregion

		private object _previousDataDirectory;
		private readonly List<ServiceNameEnum> _serviceAvailable = new List<ServiceNameEnum>();
		private readonly Dictionary<ServiceNameEnum, TestDataService> _testDataServices = new Dictionary<ServiceNameEnum, TestDataService>();


        public delegate void EnsureDatabaseInitializedDelegate(Func<DbContext> createContext, Action intializer = null, Action<DbContext> postInitDb = null);



		public void AddService(ServiceNameEnum service)
		{
			if (!_serviceAvailable.Contains(service))
			{
				_serviceAvailable.Add(service);
			}
		}

		public void CreateDb(EnsureDatabaseInitializedDelegate del)
		{
			_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
			AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetTempPath());
			foreach (var serviceAvailable in _serviceAvailable)
			{
				var service = serviceAvailable;
				var dbContext = GetDbContext(service);
				if (dbContext != null)
				{
					del(() => dbContext, () => Initialize(service));
				}
			}
		}

		public void InitService()
		{
			foreach (var serviceName in _serviceAvailable)
			{
				var service = GetService(serviceName);
				if (service != null)
				{
					_testDataServices.Add(serviceName, GetService(serviceName));
				}
			}
		}

		public void Clean()
		{
            try
            {
                foreach (var serviceAviable in _serviceAvailable)
                {
                    var dbContext = GetDbContext(serviceAviable);
                    if (dbContext != null)
                    {
                        dbContext.Database.Delete();
                    }
                }

                foreach (var serviceName in _serviceAvailable)
                {
                    var service = GetService(serviceName);
                    if (service != null)
                    {
                        service.Dispose();
                    }
                }
                _serviceAvailable.Clear();
                _testDataServices.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Exception while disposing test: " + ex);
            }
			finally 
			{
				AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
			}
		}

		public Uri GetUri(ServiceNameEnum serviceName)
		{
			Uri retVal = null;
			if (_testDataServices.ContainsKey(serviceName))
			{
				var testDataService = _testDataServices[serviceName];
				retVal = testDataService.ServiceUri;
			}
			return retVal;
		}
	}
}
