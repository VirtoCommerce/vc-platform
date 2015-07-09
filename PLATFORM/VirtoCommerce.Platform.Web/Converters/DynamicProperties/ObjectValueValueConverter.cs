using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class ObjectValueValueConverter
    {
        public static ObjectValue ToWebModel(this DynamicPropertyObjectValue model)
        {
            var result = new ObjectValue();
            result.InjectFrom(model);
            return result;
        }

        public static DynamicPropertyObjectValue ToCoreModel(this  ObjectValue model)
        {
            var result = new DynamicPropertyObjectValue();
            result.InjectFrom(model);
            return result;
        }
    }
}
