using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyNameConverter
    {
        public static DynamicPropertyName ToModel(this DynamicPropertyNameEntity entity)
        {
            var result = new DynamicPropertyName();
            result.InjectFrom(entity);
            return result;
        }

        public static DynamicPropertyNameEntity ToEntity(this DynamicPropertyName model)
        {
            var result = new DynamicPropertyNameEntity();
            result.InjectFrom(model);
            return result;
        }
    }
}
