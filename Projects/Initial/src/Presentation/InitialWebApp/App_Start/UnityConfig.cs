using System;
using Microsoft.Practices.Unity;
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
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Asset;
using VirtoCommerce.Foundation.Data.Azure.Asset;
using VirtoCommerce.Foundation.Data.Azure.Common;
using VirtoCommerce.Foundation.Data.Azure.CQRS;
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
using VirtoCommerce.Foundation.Marketing.Model.Policies;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Orders.StateMachines;
using VirtoCommerce.Foundation.Reporting.Services;
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
using VirtoCommerce.Scheduling.Jobs;
using VirtoCommerce.Search.Index;
using VirtoCommerce.Search.Providers.Elastic;
using VirtoCommerce.Search.Providers.Lucene;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Client.Caching.Interfaces;
using VirtoCommerce.Web.Client.Security;
using VirtoCommerce.Web.Client.Services.Assets;
using VirtoCommerce.Web.Client.Services.Cache;
using VirtoCommerce.Web.Client.Services.Emails;
using VirtoCommerce.Web.Client.Services.Listeners;
using VirtoCommerce.Web.Client.Services.Security;
using VirtoCommerce.Web.Client.Services.Sequences;
using VirtoCommerce.Web.Client.Services.Templates;
using IEvaluationPolicy = VirtoCommerce.Foundation.Marketing.Model.IEvaluationPolicy;

namespace Initial.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            #region Common Settings for Web and Services

            // this section is common for both web application and services application and should be kept identical
            var isAzure = AzureCommonHelper.IsAzureEnvironment();

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
            if (isAzure)
            {
                container.RegisterType<IQueueWriter, AzureQueueWriter>();
                container.RegisterType<IQueueReader, AzureQueueReader>();
            }
            else
            {
                container.RegisterType<IQueueWriter, InMemoryQueueWriter>();
                container.RegisterType<IQueueReader, InMemoryQueueReader>();
            }
            container.RegisterType<IMessageSender, DefaultMessageSender>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICurrencyService, CurrencyService>(new ContainerControlledLifetimeManager());

            //container.RegisterType<ICacheProvider, InMemCachingProvider>();
            container.RegisterType<ICacheRepository, HttpCacheRepository>();
            container.RegisterType<IOperationLogRepository, OperationLogContext>(new PerRequestLifetimeManager());
            container.RegisterType<ILogOperationFactory, LogOperationFactory>();

            //Register Sequences
            container.RegisterType<ISequenceService, SequenceService>();

            //Register Template and Email service
            container.RegisterType<ITemplateService, TemplateService>();

            if (isAzure)
            {
                container.RegisterType<IEmailService, AzureEmailService>();
            }
            else
            {
                container.RegisterType<IEmailService, NetEmailService>();
            }

            container.RegisterType<ICatalogOutlineBuilder, CatalogOutlineBuilder>();

            #region Interceptors

            // register interceptors
            container.RegisterType<IInterceptor, AuditChangeInterceptor>("audit");
            container.RegisterType<IInterceptor, LogInterceptor>("log");
            container.RegisterType<IInterceptor, EntityEventInterceptor>("events");

            #endregion

            #region Marketing

            container.RegisterType<IMarketingRepository, EFMarketingRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>();
            container.RegisterType<IPromotionUsageProvider, PromotionUsageProvider>();
            container.RegisterType<IPromotionEntryPopulate, PromotionEntryPopulate>();

            //Register prmotion evaluation policies
            container.RegisterType<IEvaluationPolicy, GlobalExclusivityPolicy>("global");
            container.RegisterType<IEvaluationPolicy, GroupExclusivityPolicy>("group");
            container.RegisterType<IEvaluationPolicy, CartSubtotalRewardCombinePolicy>("cart");
            container.RegisterType<IEvaluationPolicy, ShipmentRewardCombinePolicy>("shipment");


            container.RegisterType<IPromotionEvaluator, DefaultPromotionEvaluator>();

            #endregion

            #region Search

            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            container.RegisterInstance<ISearchConnection>(searchConnection);
            container.RegisterType<ISearchService, SearchService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISearchIndexController, SearchIndexController>();
            container.RegisterType<IBuildSettingsRepository, EFSearchRepository>(new PerRequestLifetimeManager());
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

            #region Assets

            container.RegisterType<IAssetEntityFactory, AssetEntityFactory>();

            // using azure assets
            if (isAzure)
            {
                container.RegisterType<IAssetRepository, AzureBlobAssetRepository>();
                container.RegisterType<IBlobStorageProvider, AzureBlobAssetRepository>();
                container.RegisterType<IAssetUrl, AzureBlobAssetRepository>();
            }
            else
            {
                // using local storage assets
                container.RegisterType<IAssetRepository, FileSystemBlobAssetRepository>();
                container.RegisterType<IBlobStorageProvider, FileSystemBlobAssetRepository>();
                container.RegisterType<IAssetUrl, FileSystemAssetUrl>();
            }

            container.RegisterType<IAssetService, AssetService>();
            #endregion

            #region Catalog
            container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICatalogRepository, EFCatalogRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IPricelistRepository, EFCatalogRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ICatalogService, CatalogService>();
            container.RegisterType<IPriceListAssignmentEvaluator, PriceListAssignmentEvaluator>();
            container.RegisterType<IPriceListAssignmentEvaluationContext, PriceListAssignmentEvaluationContext>();

            container.RegisterType<IImportJobEntityFactory, ImportJobEntityFactory>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IImportRepository, EFImportingRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IImportService, ImportService>();
            #endregion

            #region Customer

            container.RegisterType<ICustomerEntityFactory, CustomerEntityFactory>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<ICustomerRepository, EFCustomerRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ICustomerSessionService, CustomerSessionService>();

            #endregion

            #region Inventory

            container.RegisterType<IInventoryEntityFactory, InventoryEntityFactory>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IInventoryRepository, EFInventoryRepository>(new PerRequestLifetimeManager());

            #endregion

            #region Order

            container.RegisterType<IOrderEntityFactory, OrderEntityFactory>(new ContainerControlledLifetimeManager());

            var activityProvider = WorkflowConfiguration.Instance.DefaultActivityProvider;
            var workflowService = new WFWorkflowService(activityProvider);
            container.RegisterInstance<IWorkflowService>(workflowService);
            container.RegisterType<IOrderStateController, OrderStateController>();

            container.RegisterType<IOrderRepository, EFOrderRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IShippingRepository, EFOrderRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IPaymentMethodRepository, EFOrderRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ITaxRepository, EFOrderRepository>(new PerRequestLifetimeManager());

            container.RegisterType<ICountryRepository, EFOrderRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IOrderService, OrderService>();

            #endregion

            #region Review

            container.RegisterType<IReviewEntityFactory, ReviewEntityFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IReviewRepository, EFReviewRepository>(new PerRequestLifetimeManager());

            #endregion

            #region Security

            container.RegisterType<ISecurityEntityFactory, SecurityEntityFactory>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ISecurityRepository, EFSecurityRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IUserSecurity, WebUserSecurity>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<ISecurityService, SecurityService>();
            container.RegisterType<IOAuthWebSecurity, OAuthWebSecurityWrapper>();

            #endregion

            #region Store

            container.RegisterType<IStoreEntityFactory, StoreEntityFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IStoreService, StoreService>();
            container.RegisterType<IStoreRepository, EFStoreRepository>(new PerRequestLifetimeManager());

            #endregion

            #region Reporting

            container.RegisterType<IReportingService, ReportingService>();
            #endregion

            #endregion

            #region MVC Helpers

            //container.RegisterType<MarketingHelper>();
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
            container.RegisterType<SequencesClient>();
            container.RegisterType<SeoKeywordClient>(new PerRequestLifetimeManager());
            container.RegisterType<ReviewClient>();
            //container.RegisterType<IPaymentOption, CreditCardOption>("creditcard");

            #endregion

            #region DynamicContent

            container.RegisterType<IDynamicContentService, DynamicContentService>();
            container.RegisterType<IDynamicContentRepository, EFDynamicContentRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IDynamicContentEvaluator, DynamicContentEvaluator>();

            #endregion

            #region AppConfig

            container.RegisterType<IAppConfigRepository, EFAppConfigRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IAppConfigEntityFactory, AppConfigEntityFactory>(new ContainerControlledLifetimeManager());

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
            container.RegisterType<IEntityEventContext, EntityEventContext>(new PerRequestLifetimeManager());

            #endregion

            #region Jobs

            container.RegisterType<GenerateSearchIndexWork>();
            container.RegisterType<ProcessOrderStatusWork>();
            container.RegisterType<ProcessSearchIndexWork>();

            #endregion

            #region OutputCache

            container.RegisterType<IKeyBuilder, KeyBuilder>(new PerRequestLifetimeManager());
            container.RegisterType<IKeyGenerator, KeyGenerator>(new PerRequestLifetimeManager());
            container.RegisterType<IDonutHoleFiller, DonutHoleFiller>(new PerRequestLifetimeManager());
            container.RegisterType<ICacheHeadersHelper, CacheHeadersHelper>(new PerRequestLifetimeManager());
            container.RegisterType<ICacheSettingsManager, CacheSettingsManager>(new PerRequestLifetimeManager());
            container.RegisterType<IReadWriteOutputCacheManager, OutputCacheManager>(new PerRequestLifetimeManager());
            container.RegisterInstance<IActionSettingsSerialiser>(new EncryptingActionSettingsSerialiser(new ActionSettingsSerialiser(), new Encryptor()));
            container.RegisterType<ICacheService, CacheService>();

            #endregion

            #region Globalization

            //For using database resources
            container.RegisterType<IElementRepository, DatabaseElementRepository>(new PerRequestLifetimeManager());
            //For using Local Resources
            //container.RegisterInstance<IElementRepository>(new CacheElementRepository(new XmlElementRepository()));

            #endregion

            container.RegisterInstance(container);
        }
    }
}
