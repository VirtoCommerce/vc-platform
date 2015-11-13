using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ProductAssociationConverter
    {
        public static ProductAssociation ToWebModel(this VirtoCommerceCatalogModuleWebModelProductAssociation association)
        {
            var retVal = new ProductAssociation();
            retVal.InjectFrom(association);
            return retVal;
        }
    }
}