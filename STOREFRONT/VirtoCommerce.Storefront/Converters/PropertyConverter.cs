using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PropertyConverter
    {
        public static Property ToWebModel(this VirtoCommerceCatalogModuleWebModelProperty property)
        {
            var retVal = new Property();
            retVal.InjectFrom(property);
            if(property.Values != null)
                retVal.Values = property.Values.Select(v => v.ToWebModel()).ToList();
            return retVal;
        }

        public static PropertyValue ToWebModel(this VirtoCommerceCatalogModuleWebModelPropertyValue propertyValue)
        {
            var retVal = new PropertyValue();
            retVal.InjectFrom(propertyValue);
            return retVal;
        }
    }
}