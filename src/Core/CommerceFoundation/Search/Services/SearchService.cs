using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search.Repositories;

namespace VirtoCommerce.Foundation.Search.Services
{
	public class SearchService : ISearchService
	{
		protected IBuildSettingsRepository BuildSettingsRepository;
        protected ISearchIndexController SearchIndexController;
        protected ISearchProvider SearchProvider;


		public SearchService(IBuildSettingsRepository buildSettingsRepository, ISearchIndexController searchIndexController, ISearchProvider searchProvider)
		{
			BuildSettingsRepository = buildSettingsRepository;
            SearchIndexController = searchIndexController;
            SearchProvider = searchProvider;
		}
   
		public void BuildIndex(string scope, string indexDocumentType, bool rebuild)
		{
            SearchIndexController.Prepare(scope, indexDocumentType, rebuild);
            SearchIndexController.Process(scope, indexDocumentType);
		}

		public ISearchResults Search(string scope, ISearchCriteria criteria)
		{
            return SearchProvider.Search(scope, criteria);
		}
	}
}
