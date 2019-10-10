using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertySearchService
    {
        Task<DynamicPropertySearchResult> SearchDynamicPropertiesAsync(DynamicPropertySearchCriteria criteria);
    }
}
