using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CartConverter
    {
        public static Cart ToShopifyModel(this StorefrontModel.Cart.ShoppingCart cart, StorefrontModel.WorkContext workContext)
        {
            var result = new Cart();

            //result.Items = cart.Items.Select(x => x.ToShopifyModel(workContext)).ToList();
            //result.ItemCount = cart.Items.Count();
            //result.Note = cart.Comment;
            //result.TotalPrice = cart.Total.Amount;
            //result.TotalWeight = cart.Weight;

            return result;
        }

        public static LineItem ToShopifyModel(this StorefrontModel.Cart.LineItem lineItem, StorefrontModel.WorkContext workContext)
        {
            var result = new LineItem();

            //result.Product = lineItem.Product.ToShopifyModel(workContext);

            //result.Fulfillment = null; // TODO
            //result.Grams = lineItem.Weight;
            //result.Id = lineItem.Id;
            //result.Image = lineItem.Product.PrimaryImage != null ? lineItem.Product.PrimaryImage.ToShopifyModel() : null;
            //result.LinePrice = lineItem.ExtendedPrice.Amount;
            //result.Price = lineItem.PlacedPrice.Amount;
            //result.ProductId = lineItem.ProductId;
            ////result.Properties = null; // TODO
            //result.Quantity = lineItem.Quantity;
            //result.RequiresShipping = lineItem.RequiredShipping;
            //result.Sku = lineItem.Sku;
            //result.Taxable = lineItem.TaxIncluded;
            //result.Title = lineItem.Name;
            //result.Type = null; // TODO
            //result.Url = result.Product.Url;
            //result.Variant = lineItem.Product.ToVariant(workContext);
            //result.VariantId = lineItem.Product.Id;
            //result.Vendor = lineItem.Product.Vendor;

            return result;
        }
    }
}