using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Services;

namespace VirtoCommerce.Foundation.Catalogs.Services
{
	[UnityInstanceProviderServiceBehaviorAttribute]
	public class CatalogService : ICatalogService
	{
		private ISearchService _searchService;
		private ICatalogRepository _catalogRepository;

		public CatalogService(ICatalogRepository catalogRepository, ISearchService searchService)
		{
			_searchService = searchService;
			_catalogRepository = catalogRepository;
		}

		#region ICatalogService Members
		public CatalogItemSearchResults SearchItems(string scope, CatalogItemSearchCriteria criteria)
		{
			var results = _searchService.Search(scope, criteria) as SearchResults;
			var items = results.GetKeyAndOutlineFieldValueMap<string>();

			var r = new CatalogItemSearchResults(criteria, items, results);
			return r;
		}

		#endregion
	}
}
