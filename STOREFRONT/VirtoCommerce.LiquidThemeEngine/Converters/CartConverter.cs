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

            result.Items = cart.Items.Select(x => x.ToShopifyModel(workContext)).ToList();
            result.ItemCount = cart.Items.Count();
            result.Note = cart.Comment;
            result.TotalPrice = cart.SubTotal.Amount * 100;
            result.TotalWeight = cart.Weight;

            return result;
        }
    }
}