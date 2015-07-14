using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class ObjectValueValueConverter
    {
        public static ObjectValue ToWebModel(this DynamicPropertyObjectValue coreModel)
        {
            var result = new ObjectValue();
            result.InjectFrom(coreModel);
            result.Property = coreModel.Property.ToWebModel();
            return result;
        }
    }
}
