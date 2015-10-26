using System.Linq;
using Newtonsoft.Json.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

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
    }
}