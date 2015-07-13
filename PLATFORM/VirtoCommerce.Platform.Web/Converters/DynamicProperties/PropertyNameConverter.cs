using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class PropertyNameConverter
    {
        public static PropertyName ToWebModel(this DynamicPropertyName coreModel)
        {
            var result = new PropertyName();
            result.InjectFrom(coreModel);
            return result;
        }

        public static DynamicPropertyName ToCoreModel(this PropertyName webModel)
        {
            var result = new DynamicPropertyName();
            result.InjectFrom(webModel);
            return result;
        }
    }
}
