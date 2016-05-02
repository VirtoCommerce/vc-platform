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
        /// Returns  true  if an HTML form with the attribute  action="/search"  was submitted successfully. This allows you to show content based on whether a search was performed or not.
        /// </summary>
        public bool Performed { get; set; }
        /// <summary>
        /// Returns an array of matching search result items. The items in the array can be a(n): product, article, page
        /// </summary>
        public IMutablePagedList<Drop> Results { get; set; }

        /// <summary>
        /// Returns the number of results found.
        /// </summary>
        public int ResultsCount { get { return Results.GetTotalCount(); } }

        /// <summary>
        /// Returns the string that was entered in the search input box. Use the highlight filter to apply a different 
        /// </summary>
        public string Terms { get; set; }
    }
}
