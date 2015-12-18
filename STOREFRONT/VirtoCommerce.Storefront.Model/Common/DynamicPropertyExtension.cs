using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public static class DynamicPropertiesExtensions
    {
        public static string GetDynamicPropertyValue(this IEnumerable<DynamicProperty> properties,  string propertyName, Language language = null)
        {
            string retVal = null;
            language = language ?? Language.InvariantLanguage;
            if (properties != null)
            {
                var property = properties.FirstOrDefault(v => String.Equals(v.Name, propertyName, StringComparison.CurrentCultureIgnoreCase) && v.Values != null);

                if (property != null)
                {
                    retVal = property.Values.Where(x => x.Language.Equals(language)).Select(x => x.Value).FirstOrDefault();
                }
            }
            return retVal;
        }

        public static DynamicPropertyDictionaryItem GetDynamicPropertyDictValue(this IEnumerable<DynamicProperty> properties, string propertyName)
        {
            var retVal = new DynamicPropertyDictionaryItem();

            if (properties != null)
            {
                var property = properties.FirstOrDefault(v => String.Equals(v.Name, propertyName, StringComparison.CurrentCultureIgnoreCase) && v.IsDictionary && v.Values != null);

                if (property != null)
                {
                    retVal = property.DictionaryValues.FirstOrDefault();
                }
            }
            return retVal;
        }
    }
}
