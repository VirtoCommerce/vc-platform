using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DynamicPropertyConverter
    {
        public static DynamicProperty ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty dynamicObjectProperty)
        {
            var webModel = new DynamicProperty();

            webModel.InjectFrom(dynamicObjectProperty);

            if (dynamicObjectProperty.Values != null)
            {
                webModel.Values = dynamicObjectProperty.Values.Select(v => v.Value.ToString()).ToList();
            }

            return webModel;
        }

        public static DynamicPropertyObjectValue ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue dynamicPropertyObjectValue)
        {
            var webModel = new DynamicPropertyObjectValue();

            webModel.InjectFrom(dynamicPropertyObjectValue);

            return webModel;
        }

        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty ToServiceModel(this DynamicProperty dynamicProperty)
        {
            var serviceModel = new VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty();

            serviceModel.InjectFrom(dynamicProperty);

            if (dynamicProperty.Values != null)
            {
                serviceModel.Values = dynamicProperty.Values.Select(v => v.ToServiceModel()).ToList();
            }

            return serviceModel;
        }

        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue ToServiceModel(this string dynamicPropertyObjectValue)
        {
            var serviceModel = new VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue();

            serviceModel.Value = dynamicPropertyObjectValue;

            return serviceModel;
        }
    }
}