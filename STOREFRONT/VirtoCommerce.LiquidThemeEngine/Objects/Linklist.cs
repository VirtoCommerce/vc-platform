using DotLiquid;
using System.Collections.Generic;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents link list in site navigation part
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/linklist
    /// </remarks>
    public class Linklist : Drop
    {
        /// <summary>
        /// Returns the handle of the linklist
        /// </summary>
        public string Handle { get; set; }

        /// <summary>
        /// Returns the id of the linklist
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Returns a collection of links in the linklist
        /// </summary>
        public ICollection<Link> Links { get; set; }

        /// <summary>
        /// Returns the title of the linklist
        /// </summary>
        public string Title { get; set; }
    }
}