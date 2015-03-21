using VirtoCommerce.CatalogModule.Model;
namespace VirtoCommerce.CatalogModule.Services
{
	public interface ICatalogSearchService
	{
		SearchResult Search(SearchCriteria criteria);
	}
}
