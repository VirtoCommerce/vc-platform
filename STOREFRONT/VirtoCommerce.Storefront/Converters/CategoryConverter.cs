using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CategoryConverter
    {
        public static Category ToWebModel(this VirtoCommerceCatalogModuleWebModelCategory category, VirtoCommerceCatalogModuleWebModelProduct[] products = null)
        {
            var retVal = new Category();
            retVal.InjectFrom(category);

            if (category.Parents != null)
                retVal.Parents = category.Parents;

            if (category.SeoInfos != null)
                retVal.SeoInfos = category.SeoInfos.Select(s => s.ToWebModel()).ToArray();

            if (category.Images != null)
                retVal.Images = category.Images.Select(s => s.ToWebModel()).ToArray();

            if (category.Children != null)
                retVal.Children = category.Children.Select(c => c.ToWebModel(null)).ToArray();

            if (products != null)
                retVal.Products = products.Select(p => p.ToWebModel()).ToArray();


            return retVal;
        }
    }
}