using System;
using System.IO;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Inventories;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Frameworks.CQRS.Engines;
using VirtoCommerce.Foundation.Frameworks.CQRS.Factories;
using VirtoCommerce.Foundation.Frameworks.CQRS.Observers;
using VirtoCommerce.Foundation.Frameworks.CQRS.Senders;
using VirtoCommerce.Foundation.Frameworks.CQRS.Serialization;
using VirtoCommerce.Foundation.Frameworks.Logging;
using VirtoCommerce.Foundation.Frameworks.Logging.Factories;
using VirtoCommerce.Foundation.Frameworks.Workflow;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.CQRS;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Search.Services;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Foundation.Stores.Services;
using VirtoCommerce.Search.Index;
using VirtoCommerce.Search.Providers.Elastic;

namespace CommerceFoundation.UI.FunctionalTests.Fulfillment
{
	public class TaxesTestBase //: DbTestBase
	{
		public readonly UnityContainer Container;

		protected TaxesTestBase()
		{
			//_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
			AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetTempPath());
			//_Service = new TestDataService(typeof(TestDSOrderService));
			Container = GetLocalContainer();
			ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
		}
		
		protected IServiceLocator Locator
		{
			get
			{
				ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
				return ServiceLocator.Current;
			}
		}

		private static UnityContainer GetLocalContainer()
		{
			var container = new UnityContainer();
			container.RegisterInstance<IUnityContainer>(container);
			container.RegisterType<IKnownSerializationTypes, CatalogEntityFactory>("catalog", new ContainerControlledLifetimeManager());
			container.RegisterInstance<IConsumerFactory>(new DomainAssemblyScannerConsumerFactory(container));
			container.RegisterType<IKnownSerializationTypes, DomainAssemblyScannerConsumerFactory>("scaned", new ContainerControlledLifetimeManager(), new InjectionConstructor(container));
			container.RegisterType<IConsumerFactory, DomainAssemblyScannerConsumerFactory>();
			container.RegisterType<ISystemObserver, NullSystemObserver>();
			container.RegisterType<IEngineProcess, SingleThreadConsumingProcess>();
			container.RegisterType<IMessageSerializer, DataContractMessageSerializer>();
			container.RegisterType<IQueueWriter, InMemoryQueueWriter>();
			container.RegisterType<IQueueReader, InMemoryQueueReader>();
			container.RegisterType<IMessageSender, DefaultMessageSender>(new ContainerControlledLifetimeManager());
			container.RegisterType<ISearchService, SearchService>();
			container.RegisterType<ISearchProvider, ElasticSearchProvider>();
			container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
			container.RegisterType<ISearchIndexBuilder, CatalogItemIndexBuilder>("catalogitem");
			container.RegisterType<IOperationLogRepository, OperationLogContext>();
			container.RegisterType<ILogOperationFactory, LogOperationFactory>();
			container.RegisterType<IBuildSettingsRepository, EFSearchRepository>();
			container.RegisterType<ISearchEntityFactory, SearchEntityFactory>(new ContainerControlledLifetimeManager());
			container.RegisterType<ISearchIndexController, SearchIndexController>();
			container.RegisterType<ICacheRepository, HttpCacheRepository>();

			#region Marketing
			container.RegisterType<IMarketingRepository, EFMarketingRepository>();
			container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>();
			container.RegisterType<IPromotionUsageProvider, PromotionUsageProvider>();
			container.RegisterType<IPromotionEntryPopulate, PromotionEntryPopulate>();
			#endregion

			#region Catalog
			container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<ICatalogRepository, EFCatalogRepository>();
			container.RegisterType<IPricelistRepository, EFCatalogRepository>();
			container.RegisterType<ICatalogService, CatalogService>();

			container.RegisterType<IPriceListAssignmentEvaluator, PriceListAssignmentEvaluator>();
			container.RegisterType<IPriceListAssignmentEvaluationContext, PriceListAssignmentEvaluationContext>();
			#endregion

			#region Customer
			container.RegisterType<ICustomerEntityFactory, CustomerEntityFactory>(new ContainerControlledLifetimeManager());
			container.RegisterType<ICustomerRepository, EFCustomerRepository>();
			container.RegisterType<ICustomerSessionService, CustomerSessionService>();
			#endregion

			#region Inventory
			container.RegisterType<IInventoryEntityFactory, InventoryEntityFactory>(new ContainerControlledLifetimeManager());
			container.RegisterType<IInventoryRepository, EFInventoryRepository>();
			#endregion

			#region Order
			container.RegisterType<IOrderEntityFactory, OrderEntityFactory>(new ContainerControlledLifetimeManager());

			var activityProvider = WorkflowConfiguration.Instance.DefaultActivityProvider;
			var workflowService = new WFWorkflowService(activityProvider);
			container.RegisterInstance<IWorkflowService>(workflowService);

			container.RegisterType<IOrderRepository, EFOrderRepository>();
			container.RegisterType<ITaxRepository, EFOrderRepository>();
			container.RegisterType<IShippingRepository, EFOrderRepository>();
			container.RegisterType<IPaymentMethodRepository, EFOrderRepository>();

			container.RegisterType<ICountryRepository, EFOrderRepository>();
			container.RegisterType<IOrderService, OrderService>();
			#endregion

			#region Store
			container.RegisterType<IStoreEntityFactory, StoreEntityFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IStoreService, StoreService>();
			container.RegisterType<IStoreRepository, EFStoreRepository>();
			#endregion

			return container;
		}

		protected const string msg =
"Could not find method named 'OnLoaded' on object of type '{0}' that matches the expected signature.";

	}
}
