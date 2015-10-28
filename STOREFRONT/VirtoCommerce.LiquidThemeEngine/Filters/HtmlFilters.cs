using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/html-filters
    /// </summary>
    public class HtmlFilters
    {
        /// <summary>
        /// Generates a script tag.
        /// {{ 'shop.js' | asset_url | script_tag }}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ScriptTag(string input)
        {
            return String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", input);
        }

        /// <summary>
        /// Generates a stylesheet tag.
        /// {{ 'shop.css' | asset_url | stylesheet_tag }}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StylesheetTag(string input)
        {
            return String.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" media=\"all\" />", input);
        }
    }
}
