using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents link list single object. Cannot be invoked on its own. It must be invoked inside a linklist.
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/link
    /// </remarks>
    public class Link : Drop
    {
        /// <summary>
        /// Returns true if the link is active, or false if the link is inactive
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Returns the variable associated to the link. The possible types are: product, collection, page, blog
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// Returns the title of the link
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the type of the link. The possible values are:
        /// collection_link: if the link points to a collection
        /// product_link: if the link points to a product page
        /// page_link: if the link points to a page
        /// blog_link: if the link points to a blog
        /// relative_link: if the link points to the search page, the home page or /collections/all
        /// http_link: if the link points to an external web page, or a type or vendor collection (ex: /collections/types?q=Pants)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Returns the URL of the link
        /// </summary>
        public string Url { get; set; }
    }
}