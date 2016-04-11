using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CatalogPropertyConverter
    {
        public static CatalogProperty ToWebModel(this VirtoCommerceCatalogModuleWebModelProperty property, Language currentLanguage)
        {
            var retVal = new CatalogProperty();
            retVal.InjectFrom(property);
            //Set display names and set current display name for requested language
            if (property.DisplayNames != null)
            {
                retVal.DisplayNames = property.DisplayNames.Select(x => new LocalizedString(new Language(x.LanguageCode), x.Name)).ToList();
                retVal.DisplayName = retVal.DisplayNames.FindWithLanguage(currentLanguage, (x)=> x.Value, null);
            }
            //if display name for requested language not set get system property name
            if (String.IsNullOrEmpty(retVal.DisplayName))
            {
                retVal.DisplayName = property.Name;
            }

            //For multilingual properties need populate LocalizedValues collection and set value for requested language
            if (property.Multilanguage ?? false)
            {
                if(property.Values != null)
                {
                    retVal.LocalizedValues = property.Values.Where(x=>x.Value != null).Select(x => new LocalizedString(new Language(x.LanguageCode), x.Value.ToString())).ToList();
                }
            }
            //Set property value
            if (property.Values != null)
            {
                var propValue = property.Values.Where(x => x.Value != null).FirstOrDefault();
                if (propValue != null)
                {
                    //Use only one prop value (reserve multi values to other scenarios)
                    retVal.Value = propValue.Value.ToString();
                    retVal.ValueId = propValue.ValueId;
                }
            }
            //Try to set value for requested language
            if (retVal.LocalizedValues.Any())
            {
                retVal.Value = retVal.LocalizedValues.FindWithLanguage(currentLanguage, x => x.Value, retVal.Value);
             }
            return retVal;
        }
     
    }
}