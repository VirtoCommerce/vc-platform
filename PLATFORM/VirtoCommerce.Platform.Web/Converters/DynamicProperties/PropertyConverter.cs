using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class PropertyConverter
    {
        public static Property ToWebModel(this DynamicProperty coreModel)
        {
            var result = new Property();
            result.InjectFrom(coreModel);
            result.ValueType = EnumUtility.SafeParse(coreModel.ValueType.ToString(), PropertyValueType.Undefined);

            if (coreModel.IsMultilingual && coreModel.DisplayNames != null)
            {
                result.DisplayNames = coreModel.DisplayNames.Select(x => x.ToWebModel()).ToArray();
            }

            return result;
        }

        public static DynamicProperty ToCoreModel(this Property webModel, string objectType)
        {
            var result = new DynamicProperty();
            result.InjectFrom(webModel);
            result.ValueType = EnumUtility.SafeParse(webModel.ValueType.ToString(), DynamicPropertyValueType.Undefined);

            if (string.IsNullOrEmpty(result.ObjectType))
                result.ObjectType = objectType;

            if (webModel.IsMultilingual && webModel.DisplayNames != null)
            {
                result.DisplayNames = webModel.DisplayNames.Select(x => x.ToCoreModel()).ToArray();
            }

            return result;
        }
    }
}
