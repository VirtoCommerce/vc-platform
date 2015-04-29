using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.InventoryModule.Web.BackgroundJobs;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.SearchModule.Data.Services;

namespace VirtoCommerce.InventoryModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
		private readonly IAppBuilder _appBuilder;
        public Module(IUnityContainer container, IAppBuilder appBuilder)
        {
            _container = container;
			_appBuilder = appBuilder;
        }

        #region IModule Members

        public void Initialize()
        {
			_container.RegisterType<ISearchIndexBuilder, CatalogItemIndexBuilder>("catalogitem");
			_container.RegisterType<ISearchIndexController, SearchIndexController>();

			var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
			_container.RegisterInstance<ISearchConnection>(searchConnection);

			_container.RegisterType<ISearchProviderManager, SearchProviderManager>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ISearchProvider, SearchProviderManager>(new ContainerControlledLifetimeManager());

        }

		public void PostInitialize()
		{
			var jobScheduler = _container.Resolve<SearchIndexJobsScheduler>();
			jobScheduler.SheduleJobs();

            var searchProviderManager = _container.Resolve<ISearchProviderManager>();

			//searchProviderManager.RegisterSearchProvider(SearchProviders.Elasticsearch.ToString(), connection => new ElasticSearchProvider(new ElasticSearchQueryBuilder(), connection));
			//searchProviderManager.RegisterSearchProvider(SearchProviders.Lucene.ToString(), connection => new LuceneSearchProvider(new LuceneSearchQueryBuilder(), connection));
			//searchProviderManager.RegisterSearchProvider(SearchProviders.AzureSearch.ToString(), connection => new AzureSearchProvider(new AzureSearchQueryBuilder(), connection));

        
            //OwinConfig.Configure(_appBuilder, _container, _connectionStringName);

		}

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
			
		}

		#endregion
	}
}
