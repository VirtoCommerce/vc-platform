using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyDictionaryItemNameConverter
    {
        public static DynamicPropertyDictionaryItemName ToModel(this DynamicPropertyDictionaryItemNameEntity entity)
        {
            var result = new DynamicPropertyDictionaryItemName();
            result.InjectFrom(entity);
            return result;
        }

        public static DynamicPropertyDictionaryItemNameEntity ToEntity(this DynamicPropertyDictionaryItemName model)
        {
            var result = new DynamicPropertyDictionaryItemNameEntity();
            result.InjectFrom(model);
            return result;
        }
    }
}
