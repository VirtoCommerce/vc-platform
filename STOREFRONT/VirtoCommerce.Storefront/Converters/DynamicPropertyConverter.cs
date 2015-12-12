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
                webModel.Values = dynamicObjectProperty.Values.Select(ToWebModel).ToList();
            }

            return webModel;
        }

        public static DynamicPropertyObjectValue ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue dynamicPropertyObjectValue)
        {
            var webModel = new DynamicPropertyObjectValue();

            webModel.InjectFrom(dynamicPropertyObjectValue);

            return webModel;
        }

        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty ToServiceModel(this DynamicObjectProperty dynamicObjectProperty)
        {
            var serviceModel = new VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty();

            serviceModel.InjectFrom(dynamicObjectProperty);

            if (dynamicObjectProperty.Values != null)
            {
                serviceModel.Values = dynamicObjectProperty.Values.Select(ToServiceModel).ToList();
            }

            return serviceModel;
        }

        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue ToServiceModel(this DynamicPropertyObjectValue dynamicPropertyObjectValue)
        {
            var serviceModel = new VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue();

            serviceModel.InjectFrom(dynamicPropertyObjectValue);

            return serviceModel;
        }
    }
}