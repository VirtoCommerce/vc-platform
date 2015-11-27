using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CartConverter
    {
        public static Cart ToShopifyModel(this storefrontModel.Cart.ShoppingCart cart, storefrontModel.WorkContext workContext)
        {
            var result = new Cart();
            result.Items = cart.Items.Select(x => x.ToShopifyModel(workContext)).ToList();
            result.ItemCount = cart.Items.Count();
            result.Note = cart.Comment;
            result.TotalPrice = cart.Total.Amount;
            result.TotalWeight = cart.Weight;
            return result;
        }

        public static LineItem ToShopifyModel(this storefrontModel.Cart.LineItem lineItem, storefrontModel.WorkContext workContext)
        {
            var result = new LineItem();
            result.Grams = lineItem.Weight;
            result.Id = lineItem.Id;
            result.Image = lineItem.Product.PrimaryImage != null ? lineItem.Product.PrimaryImage.ToShopifyModel() : null;
            result.LinePrice = lineItem.ExtendedPrice.Amount;
            result.Price = lineItem.PlacedPrice.Amount;
            result.Product = lineItem.Product.ToShopifyModel(workContext);
            result.ProductId = lineItem.ProductId;
            result.Quantity = lineItem.Quantity;
            result.RequiresShipping = lineItem.RequiredShipping;
            result.Sku = lineItem.Sku;
            result.Taxable = lineItem.TaxIncluded;
            result.Title = lineItem.Name;
            //TODO
            result.Type = "";
            result.Url = result.Product.Url;
            result.Variant = lineItem.Product.ToVariant(workContext);
            result.VariantId = lineItem.Product.Id;
            result.Vendor = lineItem.Product.Vendor;
            return result;

        }
    }
}

