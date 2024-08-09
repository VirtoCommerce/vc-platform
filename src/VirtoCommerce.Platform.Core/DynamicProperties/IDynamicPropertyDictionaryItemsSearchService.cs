using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyDictionaryItemsSearchService : ISearchService<DynamicPropertyDictionaryItemSearchCriteria, DynamicPropertyDictionaryItemSearchResult, DynamicPropertyDictionaryItem>
    {
    }
}
