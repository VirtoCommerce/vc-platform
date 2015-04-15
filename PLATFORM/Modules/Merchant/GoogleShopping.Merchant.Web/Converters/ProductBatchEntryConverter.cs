using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using googleModel = Google.Apis.ShoppingContent.v2.Data;

namespace GoogleShopping.MerchantModule.Web.Converters
{
    public static class ProductBatchEntryConverter
    {
        private const string _insertMethod = "insert";
        public static googleModel.ProductsCustomBatchRequestEntry ToBatchEntryModel(this googleModel.Product product, string method = _insertMethod)
        {
            var retVal = new googleModel.ProductsCustomBatchRequestEntry();
            if (method.Equals(_insertMethod))
                retVal.Product = product;
            else
                retVal.ProductId = product.Id;

            retVal.Method = method;

            return retVal;
        }
    }
}