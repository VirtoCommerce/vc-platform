using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.Domain.Catalog.Services
{
	public interface ICatalogSearchService
	{
		SearchResult Search(SearchCriteria criteria);
	}
}
