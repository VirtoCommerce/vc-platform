using AmazonMWSClientLib.Model.Feeds;
using System.Collections.Generic;
using VirtoCommerce.Domain.Catalog.Model;

namespace Amazon.MerchantModule.Web.Providers
{
    public interface IAmazonProductProvider
    {
        IEnumerable<Product> GetProductUpdates(IEnumerable<string> ids);
        IEnumerable<Product> GetCatalogProductsBatchRequest(string catalogId, string categoryId);
    }
}
