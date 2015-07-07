using System;
using System.Globalization;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyNameConverter
    {
        public static DynamicPropertyName ToModel(this DynamicPropertyNameEntity entity)
        {
            var result = new DynamicPropertyName
            {
                Locale = entity.Locale,
                Name = entity.Name,
            };

            return result;
        }

        public static DynamicPropertyNameEntity ToEntity(this DynamicPropertyName model)
        {
            var result = new DynamicPropertyNameEntity
            {
                Locale = model.Locale,
                Name = model.Name,
            };

            return result;
        }
    }
}
