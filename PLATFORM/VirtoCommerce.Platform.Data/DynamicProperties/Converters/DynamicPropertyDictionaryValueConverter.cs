using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyDictionaryValueConverter
    {
        public static DynamicPropertyDictionaryValueEntity ToEntity(this DynamicPropertyDictionaryValue model)
        {
            var result = new DynamicPropertyDictionaryValueEntity
            {
                Locale = model.Locale,
                Value = model.Value,
            };

            return result;
        }

        public static DynamicPropertyDictionaryValue ToModel(this DynamicPropertyDictionaryValueEntity entity)
        {
            var result = new DynamicPropertyDictionaryValue
            {
                Locale = entity.Locale,
                Value = entity.Value,
            };

            return result;
        }
    }
}
