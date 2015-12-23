using System;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.SearchModule.Data.Model;
using VirtoCommerce.SearchModule.Data.Providers.Azure;
using VirtoCommerce.SearchModule.Data.Providers.ElasticSearch;
using VirtoCommerce.SearchModule.Data.Providers.Lucene;
using VirtoCommerce.SearchModule.Data.Services;
using VirtoCommerce.SearchModule.Web.BackgroundJobs;
using VirtoCommerce.SearchModule.Web.Services;

namespace VirtoCommerce.SearchModule.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;
        private readonly IAppBuilder _appBuilder;

        public Module(IUnityContainer container, IAppBuilder appBuilder)
        {
            _container = container;
            _appBuilder = appBuilder;
        }

        #region IModule Members

        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<ISearchIndexBuilder, CatalogItemIndexBuilder>(CatalogIndexedSearchCriteria.DocType);
            _container.RegisterType<ISearchIndexController, SearchIndexController>();

            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            _container.RegisterInstance<ISearchConnection>(searchConnection);

            var searchProviderManager = new SearchProviderManager(searchConnection);
            _container.RegisterInstance<ISearchProviderManager>(searchProviderManager);
            _container.RegisterInstance<ISearchProvider>(searchProviderManager);

            _container.RegisterType<IBrowseFilterService, FilterService>();
            _container.RegisterType<IItemBrowsingService, ItemBrowsingService>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            var jobScheduler = _container.Resolve<SearchIndexJobsScheduler>();
            jobScheduler.ScheduleJobs();

            var searchProviderManager = _container.Resolve<ISearchProviderManager>();

            searchProviderManager.RegisterSearchProvider(SearchProviders.Elasticsearch.ToString(), connection => new ElasticSearchProvider(new ElasticSearchQueryBuilder(), connection));
            searchProviderManager.RegisterSearchProvider(SearchProviders.Lucene.ToString(), connection => new LuceneSearchProvider(new LuceneSearchQueryBuilder(), connection));
            searchProviderManager.RegisterSearchProvider(SearchProviders.AzureSearch.ToString(), connection => new AzureSearchProvider(new AzureSearchQueryBuilder(), connection));

         
        }

        #endregion
    }
}
