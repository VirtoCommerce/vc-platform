using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/part
    /// Each part returned by the paginate.parts array represents a link in the pagination's navigation.
    /// </summary>
    public class Part : Drop
    {
        /// <summary>
        /// Returns true if the part is a link, returns false if it is not.
        /// </summary>
        public bool IsLink { get; set; }

        /// <summary>
        /// Returns the title of the part.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the URL of the part.
        /// </summary>
        public string Url { get; set; }
    }
}
