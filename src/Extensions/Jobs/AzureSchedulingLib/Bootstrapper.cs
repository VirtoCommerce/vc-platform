#region Imports

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Client.Orders.StateMachines;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
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
using VirtoCommerce.Foundation.Frameworks.Logging;
using VirtoCommerce.Foundation.Frameworks.Logging.Factories;
using VirtoCommerce.Foundation.Frameworks.Workflow;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Orders.StateMachines;
using VirtoCommerce.Foundation.PlatformTools;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Search.Services;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Foundation.Stores.Services;
using VirtoCommerce.Scheduling.Jobs;
using VirtoCommerce.Search.Index;
using VirtoCommerce.Search.Providers.Elastic;

#endregion

namespace VirtoCommerce.Scheduling.Azure
{
    using VirtoCommerce.Foundation.Catalogs;
    using VirtoCommerce.Foundation.Data.Azure.Asset;
    using VirtoCommerce.Foundation.Data.Azure.CQRS;
    using VirtoCommerce.Search.Providers.Lucene;

    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            var container = BuildUnityContainer();
            var locator = new UnityServiceLocator(container);
            //set the CSL-compliant service locator - forces the unity initialization 
            ServiceLocator.SetLocatorProvider(() => locator);
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            #region Common Settings for Web and Services
            // this section is common for both web application and services application and should be kept identical
            var container = new UnityContainer();

            container.RegisterType<IKnownSerializationTypes, CatalogEntityFactory>("catalog", new ContainerControlledLifetimeManager());
            //container.RegisterType<IKnowSerializationTypes, OrderEntityFactory>("order", new ContainerControlledLifetimeManager(), null);
            container.RegisterInstance<IConsumerFactory>(new DomainAssemblyScannerConsumerFactory(container));
            container.RegisterType<IKnownSerializationTypes, DomainAssemblyScannerConsumerFactory>("scaned", new ContainerControlledLifetimeManager(), new InjectionConstructor(container));
            container.RegisterType<IConsumerFactory, DomainAssemblyScannerConsumerFactory>();
            container.RegisterType<ISystemObserver, NullSystemObserver>();
            container.RegisterType<IEngineProcess, SingleThreadConsumingProcess>();
            container.RegisterType<IMessageSerializer, DataContractMessageSerializer>();
            container.RegisterType<IQueueWriter, AzureQueueWriter>();
            container.RegisterType<IQueueReader, AzureQueueReader>();
            container.RegisterType<IMessageSender, DefaultMessageSender>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICurrencyService, CurrencyService>(new ContainerControlledLifetimeManager());

            //container.RegisterType<ICacheProvider, InMemCachingProvider>();
            container.RegisterType<ICacheRepository, HttpCacheRepository>();

            container.RegisterType<ILogOperationFactory, LogOperationFactory>();
            container.RegisterType<IOperationLogRepository, OperationLogContext>();

            #region Interceptors

            // register interceptors
            container.RegisterType<IInterceptor, AuditChangeInterceptor>("audit");
            //container.RegisterType<IInterceptor, LogInterceptor>("log");
            //container.RegisterType<IInterceptor, EntityEventInterceptor>("events");

            #endregion

            #region Marketing
            //Needed for RemoveExpiredPromotionReservations SystemJob
            container.RegisterType<IMarketingRepository, EFMarketingRepository>();
            container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>();
            //container.RegisterType<IPromotionUsageProvider, PromotionUsageProvider>();
            //container.RegisterType<IPromotionEntryPopulate, PromotionEntryPopulate>();
            //container.RegisterType<IDynamicContentRepository, EFDynamicContentRepository>();
            #endregion

            #region Search
            var connectionString = ConnectionHelper.GetConnectionString("SearchConnectionString");
            if (connectionString == null)
            {
                Logger.Error("connectionString is null");
            }

            var searchConnection = new SearchConnection(connectionString);
            container.RegisterInstance<ISearchConnection>(searchConnection);

            container.RegisterType<ISearchService, SearchService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISearchIndexController, SearchIndexController>();
            container.RegisterType<IBuildSettingsRepository, EFSearchRepository>();
            container.RegisterType<ISearchEntityFactory, SearchEntityFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchIndexBuilder, CatalogItemIndexBuilder>("catalogitem");

            // If provider specified as lucene, use lucene libraries, otherwise use default, which is elastic search
            if (string.Equals(searchConnection.Provider, SearchProviders.Lucene.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // Lucene Search implementation
                container.RegisterType<ISearchProvider, LuceneSearchProvider>();
                container.RegisterType<ISearchQueryBuilder, LuceneSearchQueryBuilder>();
            }
            else
            {
                container.RegisterType<ISearchProvider, ElasticSearchProvider>();
                container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
            }

            #endregion

            #region AppConfig
            container.RegisterType<IAppConfigEntityFactory, AppConfigEntityFactory>();
            container.RegisterType<IAppConfigRepository, EFAppConfigRepository>();
            #endregion

            #region Assets
            container.RegisterType<IAssetEntityFactory, AssetEntityFactory>();
            container.RegisterType<IAssetRepository, AzureBlobAssetRepository>();
            container.RegisterType<IBlobStorageProvider, AzureBlobAssetRepository>();
            container.RegisterType<IAssetService, AssetService>();
            #endregion

            #region Catalog
            container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICatalogRepository, EFCatalogRepository>();
            container.RegisterType<ICatalogOutlineBuilder, CatalogOutlineBuilder>();
            container.RegisterType<IPricelistRepository, EFCatalogRepository>();
            container.RegisterType<ICatalogService, CatalogService>();
            container.RegisterType<IPriceListAssignmentEvaluator, PriceListAssignmentEvaluator>();
            container.RegisterType<IPriceListAssignmentEvaluationContext, PriceListAssignmentEvaluationContext>();

            container.RegisterType<IImportJobEntityFactory, ImportJobEntityFactory>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IImportRepository, EFImportingRepository>();
            container.RegisterType<IImportService, ImportService>();
            #endregion

            //#region Customer
            //container.RegisterType<ICustomerEntityFactory, CustomerEntityFactory>(new ContainerControlledLifetimeManager());

            //container.RegisterType<ICustomerRepository, EFCustomerRepository>();
            //#endregion

            //#region Inventory
            //container.RegisterType<IInventoryEntityFactory, InventoryEntityFactory>(new ContainerControlledLifetimeManager());
            //#endregion

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

            #region Customer
            container.RegisterType<ICustomerEntityFactory, CustomerEntityFactory>(
     new ContainerControlledLifetimeManager());

            container.RegisterType<ICustomerRepository, EFCustomerRepository>();
            container.RegisterType<ICustomerSessionService, CustomerSessionService>();
            #endregion

            #region Review
            container.RegisterType<IReviewEntityFactory, ReviewEntityFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IReviewRepository, EFReviewRepository>();
            #endregion

            #region Security
            container.RegisterType<ISecurityEntityFactory, SecurityEntityFactory>(new ContainerControlledLifetimeManager());

            //container.RegisterType<ISecurityService, SecurityService>();
            container.RegisterType<ISecurityRepository, EFSecurityRepository>();
            #endregion


            #region Store
            container.RegisterType<IStoreEntityFactory, StoreEntityFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IStoreService, StoreService>();
            container.RegisterType<IStoreRepository, EFStoreRepository>();
            #endregion
            #endregion

            container.RegisterType<GenerateSearchIndexWork>();
            container.RegisterType<ProcessOrderStatusWork>();
            container.RegisterType<ProcessSearchIndexWork>();

            return container;
        }
    }
}