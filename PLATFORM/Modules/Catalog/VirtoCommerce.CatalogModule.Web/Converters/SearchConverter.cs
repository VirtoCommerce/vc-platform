using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class SearchConverter
    {
        public static webModel.CatalogSearchResult ToWebModel(this coreModel.SearchResult result, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new webModel.CatalogSearchResult();
            retVal.InjectFrom(result);

            if (result.Products != null)
            {
                retVal.Products = result.Products.Select(x => x.ToWebModel(blobUrlResolver)).ToArray();
            }

            if (result.Categories != null)
            {
                retVal.Categories = result.Categories.Select(x => x.ToWebModel(blobUrlResolver)).ToArray();
            }

            if (result.Aggregations != null)
            {
                retVal.Aggregations = result.Aggregations.Select(a => a.ToWebModel()).ToArray();
            }

            return retVal;
        }
    }
}
