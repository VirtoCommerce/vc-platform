using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DynamicPropertyConverter
    {
        public static DynamicProperty ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty dto)
        {
            var webModel = new DynamicProperty();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(dto);

            if (dto.DisplayNames != null)
            {
                webModel.DisplayNames = dto.DisplayNames.Select(x => new LocalizedString(new Language(x.Locale), x.Name)).ToList();
            }
            if (dto.Values != null)
            {
                if (webModel.IsDictionary)
                {
                    var dictValues = dto.Values.Where(x => x.Value != null).Select(x => x.Value)
                                               .Cast<JObject>()
                                               .Select(x=> x.ToObject<VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem>())
                                               .ToArray();

                    webModel.DictionaryValues = dictValues.Select(x => x.ToWebModel()).ToList();
                }
                else
                {
                    webModel.Values = dto.Values.Select(x => x.ToWebModel()).ToList();
                }
            }

            return webModel;
        }

        public static DynamicPropertyDictionaryItem ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyDictionaryItem dto)
        {
            var retVal = new DynamicPropertyDictionaryItem();
            retVal.InjectFrom<NullableAndEnumValueInjecter>(dto);
            if(dto.DisplayNames != null)
            {
                retVal.DisplayNames = dto.DisplayNames.Select(x=> new LocalizedString(new Language(x.Locale), x.Name)).ToList();
            }
            return retVal;
        }

        public static LocalizedString ToWebModel(this VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue dto)
        {
           return new LocalizedString(new Language(dto.Locale), dto.Value.ToString());
        }


        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty ToServiceModel(this DynamicProperty dynamicProperty)
        {
            var retVal = new VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty();

            retVal.InjectFrom<NullableAndEnumValueInjecter>(dynamicProperty);

            if (dynamicProperty.Values != null)
            {
                retVal.Values = dynamicProperty.Values.Select(v => v.ToServiceModelDynamicPropertyObjectValue()).ToList();
            }
            else if(dynamicProperty.DictionaryValues != null)
            {
                retVal.Values = dynamicProperty.DictionaryValues.Select(x => x.ToServiceModelDynamicPropertyObjectValue()).ToList();
            }
            return retVal;
        }

        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue ToServiceModelDynamicPropertyObjectValue(this DynamicPropertyDictionaryItem dictItem)
        {
            var serviceModel = new VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue();

            serviceModel.Value = dictItem;

            return serviceModel;
        }

        public static VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue ToServiceModelDynamicPropertyObjectValue(this LocalizedString dynamicPropertyObjectValue)
        {
            var serviceModel = new VirtoCommercePlatformCoreDynamicPropertiesDynamicPropertyObjectValue();

            serviceModel.Value = dynamicPropertyObjectValue.Value;
            serviceModel.Locale = dynamicPropertyObjectValue.Language.CultureName;

            return serviceModel;
        }
    }
}