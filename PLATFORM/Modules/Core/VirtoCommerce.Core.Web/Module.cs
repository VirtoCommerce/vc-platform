using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CoreModule.Web.Repositories;
using VirtoCommerce.CoreModule.Web.Search;
using VirtoCommerce.CoreModule.Web.Services;
using VirtoCommerce.Domain.Fulfillment.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Platform.Core.Modularity;
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

            using (var db = new EFSearchRepository(_connectionStringName))
            {
                new SearchDatabaseInitializer().InitializeDatabase(db);
            }
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

            #endregion

            #region Search providers

            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            _container.RegisterInstance<ISearchConnection>(searchConnection);

            _container.RegisterType<ISearchProviderManager, SearchProviderManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISearchProvider, SearchProviderManager>(new ContainerControlledLifetimeManager());

            #endregion

            #region Payment gateways manager

            _container.RegisterType<IPaymentGatewayManager, InMemoryPaymentGatewayManagerImpl>(new ContainerControlledLifetimeManager());

            #endregion


            #region Fulfillment

            _container.RegisterType<IFoundationFulfillmentRepository>(new InjectionFactory(c => new FoundationFulfillmentRepositoryImpl(_connectionStringName)));
            _container.RegisterType<IFulfillmentService, FulfillmentServiceImpl>();

            #endregion
        }

        #endregion

        #region IPostInitialize Members

        public void PostInitialize()
        {
            var searchProviderManager = _container.Resolve<ISearchProviderManager>();

            searchProviderManager.RegisterSearchProvider(SearchProviders.Elasticsearch.ToString(), connection => new ElasticSearchProvider(new ElasticSearchQueryBuilder(), connection));
            searchProviderManager.RegisterSearchProvider(SearchProviders.Lucene.ToString(), connection => new LuceneSearchProvider(new LuceneSearchQueryBuilder(), connection));
            searchProviderManager.RegisterSearchProvider(SearchProviders.AzureSearch.ToString(), connection => new AzureSearchProvider(new AzureSearchQueryBuilder(), connection));
        }

        #endregion
    }
}
