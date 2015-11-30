using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DynamicPropertyConverter
    {
        public static DynamicObjectProperty ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty dynamicObjectProperty)
        {
            var webModel = new DynamicObjectProperty();

            webModel.InjectFrom(dynamicObjectProperty);

            if (dynamicObjectProperty.Values != null)
            {
                webModel.Values = dynamicObjectProperty.Values.Select(v => v.ToWebModel()).ToList();
            }

            return webModel;
        }

        public static DynamicPropertyObjectValue ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue dynamicPropertyObjectValue)
        {
            var webModel = new DynamicPropertyObjectValue();

            webModel.InjectFrom(dynamicPropertyObjectValue);

            return webModel;
        }
    }
}