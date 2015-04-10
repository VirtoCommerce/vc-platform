using System.Web;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CoreModule.Web.Assets;
using VirtoCommerce.CoreModule.Web.Notification;
using VirtoCommerce.CoreModule.Web.Repositories;
using VirtoCommerce.CoreModule.Web.Search;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Services;
using VirtoCommerce.CoreModule.Web.Settings;
using VirtoCommerce.Domain.Fulfillment.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Asset;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.Framework.Web.Search;
using VirtoCommerce.Framework.Web.Security;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.Search.Providers.Azure;
using VirtoCommerce.Search.Providers.Elastic;
using VirtoCommerce.Search.Providers.Lucene;

namespace VirtoCommerce.CoreModule.Web
{
    public class Module : IModule, IDatabaseModule, IPostInitialize
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;
        private readonly IAppBuilder _appBuilder;

        public Module(IUnityContainer container, IAppBuilder appBuilder)
        {
            _container = container;
            _appBuilder = appBuilder;
        }

        #region IDatabaseModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var db = new SecurityDbContext(_connectionStringName))
            {
                IdentityDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new IdentitySampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new IdentityDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFSecurityRepository(_connectionStringName))
            {
                SqlSecurityDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlSecuritySampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlSecurityDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFCustomerRepository(_connectionStringName))
            {
                SqlCustomerDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCustomerSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlCustomerDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFAppConfigRepository(_connectionStringName))
            {
                SqlAppConfigDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                        initializer = new SqlAppConfigSampleDatabaseInitializer();
                        break;
                    case SampleDataLevel.Reduced:
                        initializer = new SqlAppConfigReducedSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlAppConfigDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFSearchRepository(_connectionStringName))
            {
                new SearchDatabaseInitializer().InitializeDatabase(db);
            }

            // Create Hangfire tables
            new SqlServerStorage(_connectionStringName);
        }

        #endregion

        #region IModule Members

        public void Initialize()
        {
            #region Caching

            _container.RegisterType<ICacheRepository, HttpCacheRepository>(new ContainerControlledLifetimeManager());

            #endregion

            #region Settings

            _container.RegisterType<IAppConfigRepository>(new InjectionFactory(c => new EFAppConfigRepository(_connectionStringName)));
            _container.RegisterType<ISettingsManager, SettingsManager>(new ContainerControlledLifetimeManager());

            #endregion

            #region Security

            _container.RegisterType<IFoundationSecurityRepository>(new InjectionFactory(c => new FoundationSecurityRepositoryImpl(_connectionStringName)));
            _container.RegisterType<ISecurityRepository>(new InjectionFactory(c => new EFSecurityRepository(_connectionStringName)));

            _container.RegisterType<IPermissionService, PermissionService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IApiAccountProvider, ApiAccountProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClaimsIdentityProvider, ApplicationClaimsIdentityProvider>(new ContainerControlledLifetimeManager());

            _container.RegisterType<ApplicationSignInManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>()));
            _container.RegisterType<ApplicationUserManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()));
            _container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            #endregion

            #region Search providers

            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            _container.RegisterInstance<ISearchConnection>(searchConnection);

            _container.RegisterType<ISearchProviderManager, SearchProviderManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISearchProvider, SearchProviderManager>(new ContainerControlledLifetimeManager());

            #endregion

            #region Assets

            var assetsConnection = new AssetsConnection(ConnectionHelper.GetConnectionString("AssetsConnectionString"));
            _container.RegisterInstance<IAssetsConnection>(assetsConnection);

            _container.RegisterType<IAssetEntityFactory, AssetEntityFactory>();

            _container.RegisterType<IAssetsProviderManager, AssetsProviderManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IBlobStorageProvider, AssetsProviderManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAssetRepository, AssetsProviderManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAssetUrlResolver, AssetsProviderManager>(new ContainerControlledLifetimeManager());

            #endregion

            #region Payment gateways manager

            _container.RegisterType<IPaymentGatewayManager, InMemoryPaymentGatewayManagerImpl>(new ContainerControlledLifetimeManager());

            #endregion

            #region Notification

            _container.RegisterType<INotifier, InMemoryNotifierImpl>(new ContainerControlledLifetimeManager());

            #endregion

            #region Fulfillment

            _container.RegisterType<IFoundationFulfillmentRepository>(new InjectionFactory(c => new FoundationFulfillmentRepositoryImpl(_connectionStringName)));
            _container.RegisterType<IFulfillmentService, FulfillmentServiceImpl>();

            #endregion

            OwinConfig.Configure(_appBuilder, _container, _connectionStringName);
        }

        #endregion

        #region IPostInitialize Members

        public void PostInitialize()
        {
            var searchProviderManager = _container.Resolve<ISearchProviderManager>();

            searchProviderManager.RegisterSearchProvider(SearchProviders.Elasticsearch.ToString(), connection => new ElasticSearchProvider(new ElasticSearchQueryBuilder(), connection));
            searchProviderManager.RegisterSearchProvider(SearchProviders.Lucene.ToString(), connection => new LuceneSearchProvider(new LuceneSearchQueryBuilder(), connection));
            searchProviderManager.RegisterSearchProvider(SearchProviders.AzureSearch.ToString(), connection => new AzureSearchProvider(new AzureSearchQueryBuilder(), connection));

            var assetsProviderManager = _container.Resolve<IAssetsProviderManager>();

            assetsProviderManager.RegisterProvider(AzureAssetsProvider.ProviderName, connectionString => _container.Resolve<AzureAssetsProvider>());
            assetsProviderManager.RegisterProvider(LocalAssetsProvider.ProviderName, connectionString => _container.Resolve<LocalAssetsProvider>());
        }

        #endregion
    }
}
