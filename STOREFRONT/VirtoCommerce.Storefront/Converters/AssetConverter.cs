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
    public static class AssetConverter
    {
        public static Image ToWebModel(this VirtoCommerceCatalogModuleWebModelImage image)
        {
            var retVal = new Image();
            retVal.InjectFrom(image);
            return retVal;
        }

        public static Asset ToWebModel(this VirtoCommerceCatalogModuleWebModelAsset asset)
        {
            var retVal = new Asset();
            retVal.InjectFrom(asset);
            return retVal;
        }
    }
}