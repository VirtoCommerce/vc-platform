using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PropertyConverter
    {
        public static ProductProperty ToWebModel(this VirtoCommerceCatalogModuleWebModelProperty property)
        {
            var retVal = new ProductProperty();
            retVal.InjectFrom(property);
            if (property.Values != null)
            {
                var propValue = property.Values.Where(x => x.Value != null).FirstOrDefault();
                if (propValue != null)
                {
                    //Use only one prop value (reserve multivalues to other scenarious)
                    retVal.Value = propValue.Value.ToString();
                    retVal.ValueId = propValue.ValueId;
                }
            }
            
            return retVal;
        }

     
    }
}