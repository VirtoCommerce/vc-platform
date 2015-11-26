using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.LiquidThemeEngine.Objects;

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

        /// <summary>
        /// Generates an image tag
        /// {{ 'smirking_gnome.gif' | asset_url | img_tag }}
        /// </summary>
        /// <param name="input"></param>
        /// <param name="alt"></param>
        /// <param name="css"></param>
        /// <returns></returns>
        public static string ImgTag(string input, string alt = "", string css = "")
        {
            return input == null ? null : GetImageTag(input, alt, css);
        }

             
        private static string GetImageTag(string src, string alt, string css)
        {
            return String.Format("<img src=\"{0}\" alt=\"{1}\" class=\"{2}\" />", src, alt, css);
        }
    }
}
