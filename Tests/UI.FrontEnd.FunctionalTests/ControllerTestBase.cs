using FunctionalTests.Catalogs;
using FunctionalTests.TestHelpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Moq;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Loader;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Client.Globalization.Repository;
using VirtoCommerce.Client.Orders.StateMachines;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Services;
using VirtoCommerce.Foundation.Data.Asset;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Inventories;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Frameworks.CQRS.Engines;
using VirtoCommerce.Foundation.Frameworks.CQRS.Factories;
using VirtoCommerce.Foundation.Frameworks.CQRS.Observers;
using VirtoCommerce.Foundation.Frameworks.CQRS.Senders;
using VirtoCommerce.Foundation.Frameworks.CQRS.Serialization;
using VirtoCommerce.Foundation.Frameworks.Currencies;
using VirtoCommerce.Foundation.Frameworks.Email;
using VirtoCommerce.Foundation.Frameworks.Events;
using VirtoCommerce.Foundation.Frameworks.Logging;
using VirtoCommerce.Foundation.Frameworks.Logging.Factories;
using VirtoCommerce.Foundation.Frameworks.Sequences;
using VirtoCommerce.Foundation.Frameworks.Templates;
using VirtoCommerce.Foundation.Frameworks.Workflow;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Orders.StateMachines;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.CQRS;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Search.Services;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Foundation.Stores.Services;
using VirtoCommerce.PowerShell.DatabaseSetup;
using VirtoCommerce.Search.Index;
using VirtoCommerce.Search.Providers.Elastic;
using VirtoCommerce.Web.Client;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Client.Caching.Interfaces;
using VirtoCommerce.Web.Client.Services.Assets;
using VirtoCommerce.Web.Client.Services.Cache;
using VirtoCommerce.Web.Client.Services.Emails;
using VirtoCommerce.Web.Client.Services.Listeners;
using VirtoCommerce.Web.Client.Services.Security;
using VirtoCommerce.Web.Client.Services.Templates;
using VirtoCommerce.Web.Virto.Helpers;
using VirtoCommerce.Web.Virto.Helpers.Payments;

namespace UI.FrontEnd.FunctionalTests
{
    using VirtoCommerce.Foundation.Catalogs;

    public abstract class ControllerTestBase : FunctionalTestBase, IDisposable
	{
		public const string CatalogDatabaseName = "CatalogTest";
		public const string AppConfigDatabaseName = "AppConfigTest";
		public const string StoreDatabaseName = "StoreTest";
		public const string MarketingDatabaseName = "MarketingTest";

		private readonly object _previousDataDirectory;
		private ICatalogRepository _catalogRepository;
		private IPricelistRepository _pricelistRepository;
		private IAppConfigRepository _appConfigRepository;
		private IStoreRepository _storeRepository;
		private IMarketingRepository _marketingRepository;
		private readonly Dictionary<Type, string> _createdDbs = new Dictionary<Type, string>();

		protected ControllerTestBase()
		{
			_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
			AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetTempPath());

			//HttpContext Hack
			var request = new SimpleWorkerRequest("", "", "", null, new StringWriter());
			var context = new HttpContext(request);
			HttpContext.Current = context;
		}

		public override void Init(RepositoryProvider provider)
		{
			//Fake call to init repositories
			_appConfigRepository = AppConfigRepository;
			_storeRepository = StoreRepository;
            SiteMaps.Loader = new Mock<ISiteMapLoader>().Object;
			base.Init(provider);
		}

		protected UnityContainer Container;
		protected IServiceLocator Locator
		{
			get
			{
				if (Container == null)
				{
					Container = GetLocalContainer();

					// using mvc3
					var dependencyResolver = new UnityDependencyResolver(Container);
					var locator = new UnityDependencyResolverServiceLocatorProvider(dependencyResolver);
					ServiceLocator.SetLocatorProvider(() => locator);
					DependencyResolver.SetResolver(dependencyResolver);
				}

				return ServiceLocator.Current;
			}
		}

		public void Dispose()
		{
			try
			{
				// Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
				// the temp location in which they are stored is later cleaned.
				foreach (var createdDb in _createdDbs.Keys)
				{
					var repository = (DbContext)Activator.CreateInstance(createdDb, _createdDbs[createdDb]);
					repository.Database.Delete();
				}
				
			}
			finally
			{
				AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
			}
		}

		protected IAppConfigRepository AppConfigRepository
		{
			get
			{
				if (!_createdDbs.ContainsKey(typeof(EFAppConfigRepository)))
				{

					var repository = new EFAppConfigRepository(AppConfigDatabaseName);
					EnsureDatabaseInitialized(() => repository, 
						() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFAppConfigRepository, VirtoCommerce.Foundation.Data.AppConfig.Migrations.Configuration>()));
					_createdDbs.Add(typeof(EFAppConfigRepository), AppConfigDatabaseName);
				}
				return _appConfigRepository ?? (_appConfigRepository = Locator.GetInstance<IAppConfigRepository>());
			}
		}

		protected IStoreRepository StoreRepository
		{
			get
			{
				if (!_createdDbs.ContainsKey(typeof(EFStoreRepository)))
				{

					var repository = new EFStoreRepository(StoreDatabaseName);
					EnsureDatabaseInitialized(() => repository,
						() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFStoreRepository, VirtoCommerce.Foundation.Data.Stores.Migrations.Configuration>()));
					_createdDbs.Add(typeof(EFStoreRepository), StoreDatabaseName);
				}
				return _storeRepository ?? (_storeRepository = Locator.GetInstance<IStoreRepository>());
			}
		}

		protected IMarketingRepository MarketingRepository
		{
			get
			{
				if (!_createdDbs.ContainsKey(typeof(EFMarketingRepository)))
				{

					var repository = new EFMarketingRepository(MarketingDatabaseName);
					EnsureDatabaseInitialized(() => repository,
						() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFMarketingRepository, VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion.Configuration>()));
					_createdDbs.Add(typeof(EFMarketingRepository), MarketingDatabaseName);
				}
				return _marketingRepository ?? (_marketingRepository = Locator.GetInstance<IMarketingRepository>());
			}
		}

		protected ICatalogRepository CatalogRepository
		{
			get
			{
				if (!_createdDbs.ContainsKey(typeof(EFCatalogRepository)))
				{
					var repository = new EFCatalogRepository(CatalogDatabaseName);
					EnsureDatabaseInitialized(() => repository,
						() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFCatalogRepository, VirtoCommerce.Foundation.Data.Catalogs.Migrations.Configuration>()));
					_createdDbs.Add(typeof(EFCatalogRepository), CatalogDatabaseName);
				}
				return _catalogRepository ?? (_catalogRepository = Locator.GetInstance<ICatalogRepository>());
			}
		}
		protected IPricelistRepository PricelistRepository
		{
			get
			{
				if (!_createdDbs.ContainsKey(typeof(EFCatalogRepository)))
				{
					var repository = new EFCatalogRepository(CatalogDatabaseName);
					EnsureDatabaseInitialized(() => repository,
						() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFCatalogRepository, VirtoCommerce.Foundation.Data.Catalogs.Migrations.Configuration>()));
					_createdDbs.Add(typeof(EFCatalogRepository), CatalogDatabaseName);
				}
				return _pricelistRepository ?? (_pricelistRepository = Locator.GetInstance<IPricelistRepository>());
			}
		}
		protected ICustomerSessionService CustomerSessionService { get { return Locator.GetInstance<ICustomerSessionService>(); } }
		protected ICacheRepository CacheRepository { get { return Locator.GetInstance<ICacheRepository>(); } }


		protected void CreateFullGraphCatalog(string catalogId, ref Item[] items)
		{
			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogId).WithCategory("category").WithProducts(10);
			var catalog = catalogBuilder.GetCatalog();
			items = catalogBuilder.GetItems();

			CatalogRepository.Add(catalog);

			foreach (var item in items)
			{
				CatalogRepository.Add(item);
			}

			CatalogRepository.UnitOfWork.Commit();
		}

		protected PricelistAssignment GeneratePrices(IEnumerable<Item> items, string catalogId)
		{
			var pricelist = new Pricelist
			{
				Currency = CustomerSessionService.CustomerSession.Currency,
				Name = "TestPrice",
			};

			foreach (var item in items)
			{
				var price = new Price
				{
					Sale = 100,
					List = 120,
					ItemId = item.ItemId,
					MinQuantity = 1,
					PricelistId = pricelist.PricelistId,
				};

				pricelist.Prices.Add(price);
			}

			var priceListAssignment = new PricelistAssignment
			{
				CatalogId = catalogId,
				Name = "testAssigment",
				Pricelist = pricelist,
				PricelistId = pricelist.PricelistId
			};

			PricelistRepository.Add(priceListAssignment);
			PricelistRepository.UnitOfWork.Commit();

			return priceListAssignment;
		}


		private class FakeSarchProvider : ISearchProvider
		{
			private readonly ICatalogRepository _repository;

			public FakeSarchProvider(ICatalogRepository repository)
			{
				_repository = repository;
			}

			public ISearchQueryBuilder QueryBuilder { get; private set; }
			public ISearchResults Search(string scope, ISearchCriteria criteria)
			{
				var documents = new ResultDocumentSet { TotalCount = _repository.Items.Count() };
				var docList = new List<ResultDocument>();

				foreach (var indexDoc in _repository.Items)
				{
					var document = new ResultDocument();
					//foreach (var field in indexDoc.Keys)
					//	document.Add(new DocumentField(field, indexDoc[field]));

					docList.Add(document);
				}

				documents.Documents = docList.ToArray();

				// Create search results object
                return new SearchResults(criteria, new [] { documents });
			}

			public void Index(string scope, string documentType, IDocument document)
			{
				throw new NotImplementedException();
			}

			public int Remove(string scope, string documentType, string key, string value)
			{
				throw new NotImplementedException();
			}

			public void RemoveAll(string scope, string documentType)
			{
				throw new NotImplementedException();
			}

			public void Close(string scope, string documentType)
			{
				throw new NotImplementedException();
			}

			public void Commit(string scope)
			{
				throw new NotImplementedException();
			}
		}

		private UnityContainer GetLocalContainer()
		{
			#region Common Settings for Web and Services

			var container = new UnityContainer();
			container.RegisterType<IKnownSerializationTypes, CatalogEntityFactory>("catalog",
																				   new ContainerControlledLifetimeManager
																					   ());

			//container.RegisterType<IKnowSerializationTypes, OrderEntityFactory>("order", new ContainerControlledLifetimeManager(), null);
			container.RegisterInstance<IConsumerFactory>(new DomainAssemblyScannerConsumerFactory(container));
			container.RegisterType<IKnownSerializationTypes, DomainAssemblyScannerConsumerFactory>("scaned",
																								   new ContainerControlledLifetimeManager
																									   (),
																								   new InjectionConstructor
																									   (container));



			container.RegisterType<IConsumerFactory, DomainAssemblyScannerConsumerFactory>();
			container.RegisterType<ISystemObserver, NullSystemObserver>();
			container.RegisterType<IEngineProcess, SingleThreadConsumingProcess>();
			container.RegisterType<IMessageSerializer, DataContractMessageSerializer>();

			container.RegisterType<IQueueWriter, InMemoryQueueWriter>();
			container.RegisterType<IQueueReader, InMemoryQueueReader>();

			container.RegisterType<IMessageSender, DefaultMessageSender>(new ContainerControlledLifetimeManager());
			container.RegisterType<ICurrencyService, CurrencyService>(new ContainerControlledLifetimeManager());

			//container.RegisterType<ICacheProvider, InMemCachingProvider>();
			container.RegisterType<ICacheRepository, HttpCacheRepository>();
			container.RegisterType<IOperationLogRepository, OperationLogContext>();
			container.RegisterType<ILogOperationFactory, LogOperationFactory>();

			//Register Sequences
			container.RegisterType<ISequenceService, SequenceService>();

			//Register Template and Email service
			container.RegisterType<ITemplateService, TemplateService>();

			container.RegisterType<IEmailService, NetEmailService>();


			#region Interceptors

			// register interceptors
			container.RegisterType<IInterceptor, AuditChangeInterceptor>("audit");
			container.RegisterType<IInterceptor, LogInterceptor>("log");
			container.RegisterType<IInterceptor, EntityEventInterceptor>("events");

			#endregion

			#region Marketing

			container.RegisterType<IMarketingRepository, EFMarketingRepository>(new InjectionConstructor(MarketingDatabaseName));
			container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>();
			container.RegisterType<IPromotionUsageProvider, PromotionUsageProvider>();
			container.RegisterType<IPromotionEntryPopulate, PromotionEntryPopulate>();

			#endregion

			//#region Search
			container.RegisterInstance<ISearchConnection>(
				new SearchConnection(
					ConnectionHelper.GetConnectionString("SearchConnectionString")));
			container.RegisterType<ISearchService, SearchService>(new HierarchicalLifetimeManager());
			container.RegisterType<ISearchIndexController, SearchIndexController>();
			container.RegisterType<IBuildSettingsRepository, EFSearchRepository>();
			container.RegisterType<ISearchEntityFactory, SearchEntityFactory>(new ContainerControlledLifetimeManager());
			container.RegisterType<ISearchProvider, FakeSarchProvider>();
			container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
			container.RegisterType<ISearchIndexBuilder, CatalogItemIndexBuilder>("catalogitem");

			//#endregion

			#region Assets

			container.RegisterType<IAssetEntityFactory, AssetEntityFactory>();


			// using local storage assets
			container.RegisterType<IAssetRepository, FileSystemBlobAssetRepository>();
			container.RegisterType<IBlobStorageProvider, FileSystemBlobAssetRepository>();
			container.RegisterType<IAssetUrl, FileSystemAssetUrl>();

			container.RegisterType<IAssetService, AssetService>();
			#endregion

			#region Catalog
			container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<ICatalogRepository, EFCatalogRepository>(new InjectionConstructor(CatalogDatabaseName));
            container.RegisterType<ICatalogOutlineBuilder, CatalogOutlineBuilder>();
			container.RegisterType<IPricelistRepository, EFCatalogRepository>(new InjectionConstructor(CatalogDatabaseName));
			container.RegisterType<ICatalogService, CatalogService>();
			container.RegisterType<IPriceListAssignmentEvaluator, PriceListAssignmentEvaluator>();
			container.RegisterType<IPriceListAssignmentEvaluationContext, PriceListAssignmentEvaluationContext>();

			container.RegisterType<IImportJobEntityFactory, ImportJobEntityFactory>(
				new ContainerControlledLifetimeManager());
			container.RegisterType<IImportRepository, EFImportingRepository>();
			container.RegisterType<IImportService, ImportService>();
			#endregion

			#region Customer

			container.RegisterType<ICustomerEntityFactory, CustomerEntityFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<ICustomerRepository, EFCustomerRepository>();
			container.RegisterType<ICustomerSessionService, CustomerSessionService>();

			#endregion

			#region Inventory

			container.RegisterType<IInventoryEntityFactory, InventoryEntityFactory>(
				new ContainerControlledLifetimeManager());
			container.RegisterType<IInventoryRepository, EFInventoryRepository>();

			#endregion

			#region Order

			container.RegisterType<IOrderEntityFactory, OrderEntityFactory>(new ContainerControlledLifetimeManager());

			var activityProvider = WorkflowConfiguration.Instance.DefaultActivityProvider;
			var workflowService = new WFWorkflowService(activityProvider);
			container.RegisterInstance<IWorkflowService>(workflowService);
			container.RegisterType<IOrderStateController, OrderStateController>();

			container.RegisterType<IOrderRepository, EFOrderRepository>();
			container.RegisterType<IShippingRepository, EFOrderRepository>();
			container.RegisterType<IPaymentMethodRepository, EFOrderRepository>();
			container.RegisterType<ITaxRepository, EFOrderRepository>();

			container.RegisterType<ICountryRepository, EFOrderRepository>();
			container.RegisterType<IOrderService, OrderService>();

			#endregion

			#region Review

			container.RegisterType<IReviewEntityFactory, ReviewEntityFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IReviewRepository, EFReviewRepository>();

			#endregion

			#region Security

			container.RegisterType<ISecurityEntityFactory, SecurityEntityFactory>(
				new ContainerControlledLifetimeManager());
			container.RegisterType<ISecurityRepository, EFSecurityRepository>();
            container.RegisterType<IUserIdentitySecurity, IdentityUserSecurity>();
			container.RegisterType<IAuthenticationService, AuthenticationService>();
			container.RegisterType<ISecurityService, SecurityService>();

			#endregion

			#region Store

			container.RegisterType<IStoreEntityFactory, StoreEntityFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IStoreService, StoreService>();
			container.RegisterType<IStoreRepository, EFStoreRepository>(new InjectionConstructor(StoreDatabaseName));

			#endregion

			#endregion

			#region MVC Helpers

			container.RegisterType<MarketingHelper>();
			container.RegisterType<PriceListClient>();
			container.RegisterType<StoreClient>();
			container.RegisterType<CatalogClient>();
			container.RegisterType<UserClient>();
			container.RegisterType<ShippingClient>();
			container.RegisterType<PromotionClient>();
			container.RegisterType<DynamicContentClient>();
			container.RegisterType<CountryClient>();
			container.RegisterType<OrderClient>();
			container.RegisterType<DisplayTemplateClient>();
			container.RegisterType<SettingsClient>();
			container.RegisterType<IPaymentOption, CreditCardOption>("creditcard");

			#endregion

			#region DynamicContent

			container.RegisterType<IDynamicContentService, DynamicContentService>();
			container.RegisterType<IDynamicContentRepository, EFDynamicContentRepository>();
			container.RegisterType<IDynamicContentEvaluator, DynamicContentEvaluator>();

			#endregion

			#region AppConfig

			container.RegisterType<IAppConfigRepository, EFAppConfigRepository>(new InjectionConstructor(AppConfigDatabaseName));
			container.RegisterType<IAppConfigEntityFactory, AppConfigEntityFactory>();

			#endregion

			#region DisplayTemplates

			container.RegisterType<IDisplayTemplatesService, DisplayTemplatesService>();
			container.RegisterType<IDisplayTemplateEvaluator, DisplayTemplateEvaluator>();

			#endregion

			#region Events

			// Register event listeners
			container.RegisterType<IEntityEventListener, OrderChangeEventListener>("order");
			container.RegisterType<IEntityEventListener, PublicReplyEventListener>("customer");
			container.RegisterType<IEntityEventListener, CaseChangeEventListener>("customer");
            container.RegisterType<IEntityEventListener, ReviewApprovedEventListener>("review");
			container.RegisterType<IEntityEventContext, EntityEventContext>(new ContainerControlledLifetimeManager());

			#endregion

			#region Globalization

			//For using database resources
			container.RegisterType<IElementRepository, DatabaseElementRepository>();
			//For using Local Resources
			//container.RegisterInstance<IElementRepository>(new CacheElementRepository(new XmlElementRepository()));

			#endregion

            #region OutputCache

            container.RegisterType<IKeyBuilder, KeyBuilder>();
            container.RegisterType<IKeyGenerator, KeyGenerator>();
            container.RegisterType<IDonutHoleFiller, DonutHoleFiller>();
            container.RegisterType<ICacheHeadersHelper, CacheHeadersHelper>();
            container.RegisterType<ICacheSettingsManager, CacheSettingsManager>();
            container.RegisterType<IReadWriteOutputCacheManager, OutputCacheManager>();
            container.RegisterInstance<IActionSettingsSerialiser>(new EncryptingActionSettingsSerialiser(new ActionSettingsSerialiser(), new Encryptor()));
            container.RegisterType<ICacheService, CacheService>();

            #endregion

			container.RegisterInstance<IUnityContainer>(container);
			return container;
		}
	}
}
