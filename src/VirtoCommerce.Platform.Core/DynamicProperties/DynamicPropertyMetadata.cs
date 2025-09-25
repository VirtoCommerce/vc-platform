using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties;
public static class DynamicPropertyMetadata
{
    private static IDynamicPropertySearchService SearchService { get; set; } = null;
    const int _pageSize = 100;

    public static void Initialize(IDynamicPropertySearchService searchService)
    {
        SearchService ??= searchService;
    }

    public static Task<IList<DynamicProperty>> GetProperties<Entity>()
    {
        return GetProperties(nameof(Entity));
    }

    public static async Task<IList<DynamicProperty>> GetProperties(string objectType)
    {
        ArgumentNullException.ThrowIfNull(SearchService,
            "DynamicPropertySearchService is not initialized. Call DynamicPropertyMetadata.Initialize() method in your module's InitializeAsync method.");

        var criteria = AbstractTypeFactory<DynamicPropertySearchCriteria>.TryCreateInstance();
        criteria.ObjectType = objectType;
        criteria.Take = _pageSize;

        var properties = await SearchService.SearchAllNoCloneAsync(criteria);

        return properties;
    }
}
