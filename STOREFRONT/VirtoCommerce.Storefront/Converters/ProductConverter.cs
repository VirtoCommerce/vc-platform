using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ProductConverter
    {
        public static Product ToWebModel(this VirtoCommerceCatalogModuleWebModelProduct product, List<Property> properties = null)
        {
            var retVal = new Product();

            retVal.InjectFrom(product);
            retVal.Sku = product.Code;
            if (product.Properties != null)
                retVal.Properties = product.Properties.Select(p => p.ToWebModel(properties)).ToList();

            if (product.Images != null)
                retVal.Images = product.Images.Select(i => i.ToWebModel()).ToArray();

            if (product.Assets != null)
                retVal.Assets = product.Assets.Select(a => a.ToWebModel()).ToList();

            if (product.Variations != null)
                retVal.Variations = product.Variations.Select(v => v.ToWebModel(retVal.Properties)).ToList();

            if (product.Links != null)
                retVal.Links = product.Links.Select(l => l.ToWebModel()).ToList();

            if (product.SeoInfos != null)
                retVal.SeoInfos = product.SeoInfos.Select(s => s.ToWebModel()).ToList();

            if (product.Reviews != null)
                retVal.Reviews = product.Reviews.Select(r => r.ToWebModel()).ToList();

            if (product.Associations != null && product.Associations.Any())
                retVal.Associations = product.Associations.Select(a => a.ToWebModel()).ToList();

            return retVal;
        }
    }
}