using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using FunctionalTests.TestHelpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data;
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
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
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

namespace FunctionalTests.Orders.Helpers
{
    using System.Data.Entity;

    using VirtoCommerce.PowerShell.DatabaseSetup;

    [JsonSupportBehavior]
	public class TestDSOrderService : DSOrderService
	{
		public const string DatabaseName = "OrdersTest";

        protected override EFOrderRepository CreateRepository()
		{
			return new EFOrderRepository(DatabaseName, new OrderEntityFactory());
		}
	}

	public abstract class OrderTestBase : FunctionalTestBase, IDisposable
	{
		#region Infrastructure/setup
		private TestDataService _Service;
		//private readonly TestService _OrderService;

		private readonly object _previousDataDirectory;

		protected OrderTestBase()
		{
			_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
			AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);

			//_OrderService = new TestService(typeof(OrderService));
		}

		public override void Init(RepositoryProvider provider)
		{
			if (provider == RepositoryProvider.DataService)
			{
				_Service = new TestDataService(typeof(TestDSOrderService));
			}

			base.Init(provider);
		}

		public void Dispose()
		{
			try
			{
				// Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
				// the temp location in which they are stored is later cleaned.
				using (var context = new EFOrderRepository(TestDSOrderService.DatabaseName))
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

		protected UnityContainer _container;
		protected IServiceLocator Locator
		{
			get
			{
				if (_container == null)
				{
					_container = GetLocalContainer();
					ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(_container));
				}

				return ServiceLocator.Current;
			}
		}

		protected IOrderRepository OrderRepository
		{
			get
			{
				return Locator.GetInstance<IOrderRepository>();
			}
		}

		protected ITaxRepository TaxRepository
		{
			get
			{
				return Locator.GetInstance<ITaxRepository>();
			}
		}

		protected ICacheRepository CacheRepository
		{
			get
			{
				return Locator.GetInstance<ICacheRepository>();
			}
		}

		protected OrderGroup RunWorkflow(string name, OrderGroup cart)
		{
			var service = Locator.GetInstance<IOrderService>();
			var result = service.ExecuteWorkflow(name, cart);
			//List<string> warnings = result.WorkflowResult.Warnings;
			//if (warnings != null)
			{
				//foreach (string warning in warnings)
				{
					//TODO: generate error message
					//ErrorManager.GenerateError(warning);
				}
			}
			return result.OrderGroup;
		}

		protected OrderWorkflowResult InvokeActivity(Activity activity, OrderGroup orderGroup)
		{
			var parameters = new Dictionary<string, object>();
			parameters["OrderGroupArgument"] = orderGroup;

			var retVal = new WorkflowResult();
			parameters["ResultArgument"] = retVal;

			var invoker = new WorkflowInvoker(activity);
			invoker.Extensions.Add(Locator);
			//invoker.Extensions.Add(OrderRepository);
			invoker.Invoke(parameters);

			var r = new OrderWorkflowResult(retVal) { OrderGroup = orderGroup };

			return r;
		}

		protected IOrderRepository GetRepository()
		{
            EnsureDatabaseInitialized(() => new EFOrderRepository(TestDSOrderService.DatabaseName), 
				() => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFOrderRepository, VirtoCommerce.Foundation.Data.Orders.Migrations.Configuration>()));

			if (RepositoryProvider == RepositoryProvider.DataService)
			{
				return new DSOrderClient(_Service.ServiceUri, new OrderEntityFactory(), null);
			}
			else
			{
				return new EFOrderRepository(TestDSOrderService.DatabaseName);
			}
		}

		protected void RefreshRepository(ref IOrderRepository client)
		{
			client.Dispose();
			client = null;
			GC.Collect();
			client = GetRepository();
		}

		protected static Order CreateOrderConstant()
		{
			var builder = TestOrderBuilder.BuildOrder();
			var order = builder.GetOrder();
			const string customerId = "3a6e29a3-d0c9-4a9b-8207-faf957015c60";

			builder.WithAddresess()
				   .WithShipmentCount(1, 123.23m)
				   .WithShipmentDiscount(20.11m)
				   .WithLineItemsConstant()
				   .WithReturns()
				   .WithCustomer(customerId);

			order.StoreId = "SampleStore";
			return order;
		}

		private static UnityContainer GetLocalContainer()
		{
			UnityContainer container = new UnityContainer();
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
			container.RegisterInstance<ISearchConnection>(new SearchConnection(dataSource: "localhost:9200", scope: "default"));
			#endregion

			#region Store
			container.RegisterType<IStoreEntityFactory, StoreEntityFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IStoreService, StoreService>();
			container.RegisterType<IStoreRepository, EFStoreRepository>();
			#endregion

			ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
			return container;
		}
	}
}
