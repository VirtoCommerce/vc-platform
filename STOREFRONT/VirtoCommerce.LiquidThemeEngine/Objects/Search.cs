using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid/objects/search
    /// </summary>
    public class Search : Drop
    {
        /// <summary>
        /// Returns an array of matching search result items. The items in the array can be a(n): product, article, page
        /// </summary>
        public IMutablePagedList<Drop> Results { get; set; }

        /// <summary>
        /// Returns the number of results found.
        /// </summary>
        public int ResultsCount { get; set; }

        /// <summary>
        /// Returns the string that was entered in the search input box. Use the highlight filter to apply a different 
        /// </summary>
        public string Terms { get; set; }
    }
}
