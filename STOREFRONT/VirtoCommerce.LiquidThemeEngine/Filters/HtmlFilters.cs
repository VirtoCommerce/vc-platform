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
        public static string ImgTag(object input, string alt = "", string css = "")
        {
            return input == null ? null : GetImageTag(GetImageUrl(input), alt, css);
        }

        public static string ImgUrl(object input, string type)
        {
            return input == null ? null : GetImageUrl(input);
        }

        private static string GetImageUrl(object input)
        {
            if (input is Product)
            {
                return (input as Product).FeaturedImage.Src;
            }
            if (input is Image)
            {
                return (input as Image).Src;
            }
            if (input is Variant)
            {
                return (input as Variant).Image.Src;
            }
            //if (input is Collection)
            //{
            //    return (input as Collection).Image.Src;
            //}
            //if (input is LineItem)
            //{
            //    var lineItem = input as LineItem;
            //    return lineItem.Image;
            //}
            return input.ToString();
        }

        private static string GetImageTag(string src, string alt, string css)
        {
            return String.Format("<img src=\"{0}\" alt=\"{1}\" class=\"{2}\" />", src, alt, css);
        }
    }
}
