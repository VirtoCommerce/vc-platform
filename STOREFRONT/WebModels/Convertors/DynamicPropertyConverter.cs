using System.Linq;
using Newtonsoft.Json.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Convertors
{
    public static class DynamicPropertyConverter
    {
        public static DynamicProperty ToViewModel(this DataContracts.DynamicProperties.DynamicProperty property)
        {
            var retVal = new DynamicProperty();
            retVal.InjectFrom(property);
            if (property.Values != null)
            {
                if (retVal.IsDictionary)
                {
                    retVal.Values = property.Values.Cast<JObject>().Where(x => x["value"] != null).Select(x => x["value"]["name"].ToString()).ToList();
                }
                else
                {
                    retVal.Values = property.Values.Cast<JObject>().Where(x => x["value"] != null).Select(x => x["value"].ToString()).ToList();
                }
            }

            if (property.DictionaryItems != null)
            {
                retVal.DictionaryItems = property.DictionaryItems.Select(x => x.ToViewModel()).ToList();
            }
            return retVal;
        }

        public static DynamicPropertyDictionaryItem ToViewModel(this DataContracts.DynamicProperties.DynamicPropertyDictionaryItem dictItem)
        {
            var retVal = new DynamicPropertyDictionaryItem();
            retVal.InjectFrom(dictItem);
            return retVal;
        }

        public static DataContracts.DynamicProperties.DynamicProperty ToServiceModel(this DynamicProperty dynamicProperty)
        {
            var serviceModel = new DataContracts.DynamicProperties.DynamicProperty();

            serviceModel.InjectFrom(dynamicProperty);

            if (dynamicProperty.Values != null)
            {
                serviceModel.Values = new List<object>();
                if (serviceModel.IsDictionary)
                {
                    foreach (var value in dynamicProperty.Values)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            var dictionaryItem = dynamicProperty.DictionaryItems.FirstOrDefault(i => i.Name == value);
                            if (dictionaryItem != null)
                            {
                                serviceModel.Values.Add(new { Value = dictionaryItem });
                            }
                        }
                    }
                }
                else
                {
                    foreach (var value in dynamicProperty.Values)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            serviceModel.Values.Add(new { Value = value });
                        }
                    }
                }
            }

            if (dynamicProperty.DictionaryItems != null)
            {
                serviceModel.DictionaryItems = dynamicProperty.DictionaryItems.Select(x => x.ToServiceModel()).ToList();
            }

            return serviceModel;
        }

        public static DataContracts.DynamicProperties.DynamicPropertyDictionaryItem ToServiceModel(this DynamicPropertyDictionaryItem propertyItem)
        {
            var serviceModel = new DataContracts.DynamicProperties.DynamicPropertyDictionaryItem();

            serviceModel.InjectFrom(propertyItem);

            return serviceModel;
        }
    }
}