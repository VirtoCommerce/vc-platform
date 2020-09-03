using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyDictionaryItemsSearchService
    {
        Task<DynamicPropertyDictionaryItemSearchResult> SearchDictionaryItemsAsync(DynamicPropertyDictionaryItemSearchCriteria criteria);
    }
}
