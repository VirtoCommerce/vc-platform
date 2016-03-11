using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using PagedList;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class SearchConverter
    {
        public static Search ToShopifyModel(this IMutablePagedList<Storefront.Model.Catalog.Product> products, string terms)
        {
            var retVal = new Search();
            retVal.Results = new MutablePagedList<Drop>((pageNumber, pageSize) =>
            {
               products.Slice(pageNumber, pageSize);
               retVal.ResultsCount = products.TotalItemCount;
               return new StaticPagedList<Drop>(products.Select(x=>x.ToShopifyModel()).OfType<Drop>(), products);
            });
            retVal.ResultsCount = products.TotalItemCount;
            retVal.Terms = terms;
            retVal.Performed = true;
            return retVal;
        }
    }
}
