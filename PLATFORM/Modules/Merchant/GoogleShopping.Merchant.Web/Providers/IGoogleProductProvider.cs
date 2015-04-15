using System.Collections.Generic;
using Google.Apis.ShoppingContent.v2.Data;

namespace GoogleShopping.MerchantModule.Web.Providers
{
    public interface IGoogleProductProvider
    {
        IEnumerable<Product> GetProductUpdates(IEnumerable<string> ids);
        ProductsCustomBatchRequest GetProductsBatchRequest(IEnumerable<string> ids);
        ProductsCustomBatchRequest GetCatalogProductsBatchRequest(string catalogId);
    }
}
