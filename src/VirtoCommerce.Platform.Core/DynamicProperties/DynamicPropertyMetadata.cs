using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties;
public static class DynamicPropertyMetadata
{
    private static IDynamicPropertyMetaDataResolver MetaDataResolver { get; set; } = null;

    public static void Initialize(IDynamicPropertyMetaDataResolver metaDataResolver)
    {
        MetaDataResolver ??= metaDataResolver;
    }

    public static Task<IList<DynamicProperty>> GetProperties<Entity>()
    {
        return GetProperties(nameof(Entity));
    }

    public static async Task<IList<DynamicProperty>> GetProperties(string objectType)
    {
        if (MetaDataResolver == null)
        {
            throw new InvalidOperationException("IDynamicPropertyMetaDataResolver is not initialized. Call DynamicPropertyMetadata.Initialize() method in your module's InitializeAsync method.");
        }

        var properties = await MetaDataResolver.SearchAllAsync(objectType);

        return properties;
    }
}
