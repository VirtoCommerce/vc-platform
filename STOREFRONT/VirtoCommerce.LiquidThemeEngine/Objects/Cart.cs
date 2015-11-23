using DotLiquid;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents customer's shopping cart
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/cart
    /// </remarks>
    public class Cart : Drop
    {
        private readonly Storefront.Model.WorkContext _workContext;
        private readonly Storefront.Model.Common.IStorefrontUrlBuilder _urlBuilder;
        private readonly Storefront.Model.Cart.ShoppingCart _cart;

        public Cart(
            Storefront.Model.WorkContext workContext,
            Storefront.Model.Common.IStorefrontUrlBuilder urlBuilder,
            Storefront.Model.Cart.ShoppingCart cart)
        {
            _workContext = workContext;
            _urlBuilder = urlBuilder;
            _cart = cart;
        }

        /// <summary>
        /// Gets an additional shopping cart information
        /// </summary>
        public IDictionary<string, string> Attributes
        {
            get
            {
                // TODO: Populate with dynamic properties
                return null;
            }
        }

        /// <summary>
        /// Gets collection of shopping cart line items
        /// </summary>
        public ICollection<LineItem> Items
        {
            get
            {
                return _cart.Items.Select(i => new LineItem(_workContext, _urlBuilder, i)).ToList();
            }
        }

        /// <summary>
        /// Gets the number of shopping cart line items
        /// </summary>
        public int ItemCount
        {
            get
            {
                return _cart.Items != null ? _cart.Items.Count : 0;
            }
        }

        /// <summary>
        /// Gets the shopping cart note
        /// </summary>
        public string Note
        {
            get
            {
                return _cart.Comment;
            }
        }

        /// <summary>
        /// Gets shopping cart total price
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                return _cart.Total != null ? _cart.Total.Amount : 0M;
            }
        }

        /// <summary>
        /// Gets shopping cart total weight
        /// </summary>
        public decimal TotalWeight
        {
            get
            {
                return _cart.Weight.HasValue ? _cart.Weight.Value : 0M;
            }
        }
    }
}