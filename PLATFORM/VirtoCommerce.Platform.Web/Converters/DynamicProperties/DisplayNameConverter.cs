using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class DisplayNameConverter
    {
        public static DisplayName ToWebModel(this DynamicPropertyName model)
        {
            var result = new DisplayName();
            result.InjectFrom(model);
            return result;
        }

        public static DynamicPropertyName ToCoreModel(this  DisplayName model)
        {
            var result = new DynamicPropertyName();
            result.InjectFrom(model);
            return result;
        }
    }
}
