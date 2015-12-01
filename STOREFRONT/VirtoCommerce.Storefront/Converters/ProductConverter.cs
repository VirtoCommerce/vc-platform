using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ProductConverter
    {
        public static Product ToWebModel(this VirtoCommerceCatalogModuleWebModelProduct product)
        {
            var retVal = new Product();
            retVal.Price = new ProductPrice();

            retVal.InjectFrom(product);

            retVal.Sku = product.Code;

            if(product.Category != null)
            {
                retVal.Category = product.Category.ToWebModel();
            }

            if (product.Properties != null)
                retVal.Properties = product.Properties.Select(p => p.ToWebModel()).ToList();

            if (product.Images != null)
            {
                retVal.Images = product.Images.Select(i => i.ToWebModel()).ToArray();
                retVal.PrimaryImage = retVal.Images.FirstOrDefault(x => String.Equals(x.Url, product.ImgSrc, StringComparison.InvariantCultureIgnoreCase));
            }

            if (product.Assets != null)
            {
                retVal.Assets = product.Assets.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(v => v.ToWebModel()).ToList();
            }

            if (product.SeoInfos != null)
                retVal.SeoInfo = product.SeoInfos.Select(s => s.ToWebModel()).FirstOrDefault();

            if (product.Reviews != null)
                retVal.EditorialReviews = product.Reviews.Select(r => r.ToWebModel()).ToList();

            return retVal;
        }
    }
}