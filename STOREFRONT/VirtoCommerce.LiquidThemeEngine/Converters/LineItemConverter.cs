using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class LineItemConverter
    {
        public static LineItem ToShopifyModel(this StorefrontModel.Cart.LineItem lineItem, StorefrontModel.WorkContext workContext)
        {
            var shopifyModel = new LineItem();

            //shopifyModel.Product = lineItem.Product.ToShopifyModel();
            shopifyModel.Fulfillment = null; // TODO
            shopifyModel.Grams = lineItem.Weight;
            shopifyModel.Id = lineItem.Id;
            shopifyModel.Image = new Image
            {
                Alt = lineItem.Name,
                Name = lineItem.Name,
                ProductId = lineItem.ProductId,
                Src = lineItem.ImageUrl
            };
            shopifyModel.LinePrice = lineItem.ExtendedPrice.Amount * 100;
            shopifyModel.Price = lineItem.PlacedPrice.Amount * 100;
            shopifyModel.ProductId = lineItem.ProductId;
            //shopifyModel.Properties = null; // TODO
            shopifyModel.Quantity = lineItem.Quantity;
            shopifyModel.RequiresShipping = lineItem.RequiredShipping;
            shopifyModel.Sku = lineItem.Sku;
            shopifyModel.Taxable = lineItem.TaxIncluded;
            shopifyModel.Title = lineItem.Name;
            shopifyModel.Type = null; // TODO
            shopifyModel.Url = null; // TODO
            shopifyModel.Variant = null; // TODO
            shopifyModel.VariantId = lineItem.ProductId;
            shopifyModel.Vendor = null; // TODO

            return shopifyModel;
        }

        public static LineItem ToShopifyModel(this StorefrontModel.Order.LineItem lineItem, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new LineItem
            {
                Fulfillment = null,
                Grams = lineItem.Weight ?? 0m,
                Id = lineItem.Id,
                Quantity = lineItem.Quantity ?? 0,
                Price = lineItem.Price.Amount * 100,
                ProductId = lineItem.ProductId,
                Sku = lineItem.Name,
                Title = lineItem.Name,
                Url = urlBuilder.ToAppAbsolute("/product/" + lineItem.ProductId),
            };

            result.LinePrice = result.Price * result.Quantity;

            result.Product = new Product
            {
                Id = result.ProductId,
                Url = result.Url
            };

            //result.Image = lineItem.Product.PrimaryImage != null ? lineItem.Product.PrimaryImage.ToShopifyModel() : null;
            //result.RequiresShipping = lineItem.RequiredShipping;
            //result.Taxable = lineItem.TaxIncluded;

            return result;
        }
    }
}