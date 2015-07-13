using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyDictionaryItemNameConverter
    {
        public static DynamicPropertyDictionaryItemNameEntity ToEntity(this DynamicPropertyDictionaryItemName model)
        {
            var result = new DynamicPropertyDictionaryItemNameEntity
            {
                Locale = model.Locale,
                Name = model.Name,
            };

            return result;
        }

        public static DynamicPropertyDictionaryItemName ToModel(this DynamicPropertyDictionaryItemNameEntity entity)
        {
            var result = new DynamicPropertyDictionaryItemName
            {
                Locale = entity.Locale,
                Name = entity.Name,
            };

            return result;
        }
    }
}
