using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Marketing;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ProductAssociationConverter
    {
        public static ProductAssociation ToWebModel(this VirtoCommerceCatalogModuleWebModelProductAssociation association)
        {
            var retVal = new ProductAssociation();
            retVal.InjectFrom<NullableAndEnumValueInjecter>(association);
            retVal.ProductImage = new Image() { Url = association.ProductImg };
            retVal.ProductName = association.ProductName;
            retVal.ProductSku = association.ProductCode;
            return retVal;
        }
    }
}