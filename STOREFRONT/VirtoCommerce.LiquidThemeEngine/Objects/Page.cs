using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/page
    /// </summary>
    public class Page : Drop
    {
        /// <summary>
        /// Returns the author of a page.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Returns the content of a page.
        /// </summary>
        public string Content { get; set; }

        public string Handle { get; set; }

        /// <summary>
        /// Returns the id of the page.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Returns the timestamp of when the page was created. Use the date filter to format the timestamp.
        /// </summary>
        public DateTime PublishedAt { get; set; }

        /// <summary>
        /// Returns the name of the custom page template assigned to the page, without the page. prefix nor the .liquid suffix. Returns nil if a custom template is not assigned to the page.
        /// </summary>
        public string TemplateSuffix { get; set; }

        /// <summary>
        /// Returns the title of a page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the relative URL of the page.
        /// </summary>
        public string Url { get; set; }
    }
}
