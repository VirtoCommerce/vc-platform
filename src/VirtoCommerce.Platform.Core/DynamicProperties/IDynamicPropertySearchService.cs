using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertySearchService : ISearchService<DynamicPropertySearchCriteria, DynamicPropertySearchResult, DynamicProperty>
    {
    }
}
