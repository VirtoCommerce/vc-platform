using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class PropertyConverter
    {
        public static Property ToWebModel(this DynamicProperty model)
        {
            var result = new Property();
            result.InjectFrom(model);
            result.ValueType = EnumUtility.SafeParse(model.ValueType.ToString(), PropertyValueType.Undefined);

            if (model.DisplayNames != null)
            {
                result.DisplayNames = model.DisplayNames.Select(x => x.ToWebModel()).ToArray();
            }

            return result;
        }

        public static DynamicProperty ToCoreModel(this Property model)
        {
            var result = new DynamicProperty();
            result.InjectFrom(model);
            result.ValueType = EnumUtility.SafeParse(model.ValueType.ToString(), DynamicPropertyValueType.Undefined);

            if (model.DisplayNames != null)
            {
                result.DisplayNames = model.DisplayNames.Select(x => x.ToCoreModel()).ToArray();
            }

            return result;
        }
    }
}
