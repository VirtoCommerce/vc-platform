using System.Collections.Generic;

namespace VirtoCommerce.CartModule.Web.Model
{
    public class SearchResult
    {
        public SearchResult()
        {
            ShopingCarts = new List<ShoppingCart>();
        }

        /// <summary>
        /// Gets or sets the value of search result total shopping cart count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of search result shopping carts
        /// </summary>
        /// <value>
        /// List of ShoppingCart object
        /// </value>
        public List<ShoppingCart> ShopingCarts { get; set; }
    }
}