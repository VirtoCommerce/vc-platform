using System;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.InventoryModule.Web.BackgroundJobs;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.SearchModule.Data.Provides.Azure;
using VirtoCommerce.SearchModule.Data.Provides.Elastic;
using VirtoCommerce.SearchModule.Data.Provides.Lucene;
using VirtoCommerce.SearchModule.Data.Services;

namespace VirtoCommerce.SearchModule.Web
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

			var searchProviderManager = new SearchProviderManager(searchConnection);
			_container.RegisterInstance<ISearchProviderManager>(searchProviderManager);
			_container.RegisterInstance<ISearchProvider>(searchProviderManager);

        }

		public void PostInitialize()
		{
			var jobScheduler = _container.Resolve<SearchIndexJobsScheduler>();
			//jobScheduler.SheduleJobs();

			var searchProviderManager = _container.Resolve<ISearchProviderManager>();

			searchProviderManager.RegisterSearchProvider(SearchProviders.Elasticsearch.ToString(), connection => new ElasticSearchProvider(new ElasticSearchQueryBuilder(), connection));
			searchProviderManager.RegisterSearchProvider(SearchProviders.Lucene.ToString(), connection => new LuceneSearchProvider(new LuceneSearchQueryBuilder(), connection));
			searchProviderManager.RegisterSearchProvider(SearchProviders.AzureSearch.ToString(), connection => new AzureSearchProvider(new AzureSearchQueryBuilder(), connection));

			var cacheManager = _container.Resolve<CacheManager>();
			var cacheSettings = new[] 
			{
				new CacheSettings("CatalogItemIndexBuilder.IndexItemCategories", TimeSpan.FromMinutes(30))
			};
			cacheManager.AddCacheSettings(cacheSettings);

		}

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
			
		}

		#endregion
	}
}
