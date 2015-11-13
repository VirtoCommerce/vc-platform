using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CategoryLinkConverter
    {
        public static CategoryLink ToWebModel(this VirtoCommerceCatalogModuleWebModelCategoryLink categoryLink)
        {
            var retVal = new CategoryLink();
            retVal.InjectFrom(categoryLink);
            return retVal;
        }
    }
}