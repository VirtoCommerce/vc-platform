using DotLiquid;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Cart;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Cart : Drop
    {
        private readonly ShoppingCart _cart;

        public Cart(ShoppingCart cart)
        {
            _cart = cart;
        }

        public ICollection<LineItem> Items
        {
            get
            {
                return _cart.Items.Select(i => new LineItem(i)).ToList();
            }
        }

        public int ItemCount
        {
            get
            {
                return _cart.Items != null ? _cart.Items.Count : 0;
            }
        }
    }
}