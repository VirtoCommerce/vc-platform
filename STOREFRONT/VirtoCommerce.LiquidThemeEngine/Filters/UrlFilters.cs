using System;
using System.Globalization;
using System.Linq;
using System.Web;
using DotLiquid;
using storefrontModel = VirtoCommerce.Storefront.Model;
using shopifyModel = VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/url-filters
    /// </summary>
    public class UrlFilters
    {
        /// <summary>
        /// Generates a link to the customer login page.
        /// {{ 'Log in' | customer_login_link }}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CustomerLoginLink(string input)
        {
            return BuildHtmlLink("customer_login_link", "~/account/login", input);
        }
        public static string CustomerRegisterLink(string input)
        {
            return BuildHtmlLink("customer_register_link", "~/account/register", input);
        }

        public static string CustomerLogoutLink(string input)
        {
            return BuildHtmlLink("customer_logout_link", "~/account/logout", input);
        }

        public static string EditCustomerAddressLink(string input, string id)
        {
            return BuildOnClickLink(input, "Shopify.CustomerAddress.toggleForm('{0}');return false", id);
        }

        public static string DeleteCustomerAddressLink(string input, string id)
        {
            return BuildOnClickLink(input, "Shopify.CustomerAddress.destroy('{0}');return false", id);
        }

        /// <summary>
        /// Returns the URL of an image. Accepts an image size as a parameter. The img_url filter can be used on the following objects:
        /// product, variant,  line item, collection, image
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ImgUrl(object input, string type = null)
        {
            if (input == null)
                return null;

            var retVal = input.ToString();
            var product = input as shopifyModel.Product;
            var image = input as shopifyModel.Image;
            var variant = input as shopifyModel.Variant;
            var collection = input as shopifyModel.Collection;

            if (product != null)
            {
                retVal = product.FeaturedImage != null ? product.FeaturedImage.Src : null;
            }
            if (image != null)
            {
                retVal = image.Src;
            }
            if (variant != null)
            {
                retVal = variant.FeaturedImage != null ? variant.FeaturedImage.Src : null;
            }
            if (collection != null)
            {
                retVal = collection.Image != null ? collection.Image.Src : null;
            }

            return retVal;
        }

        /// <summary>
        /// Generates an HTML link. The first parameter is the URL of the link, and the optional second parameter is the title of the link.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LinkTo(object input, string link, string title = "")
        {
            return String.Format("<a href=\"{0}\" title=\"{1}\">{2}</a>", link, title, input);
        }

        /// <summary>
        /// Returns the URL of a file in the "assets" folder of a theme.
        /// {{ 'shop.css' | asset_url }}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AssetUrl(string input)
        {
            string retVal = null;
            if (input != null)
            {
                var themeAdaptor = (ShopifyLiquidThemeEngine)Template.FileSystem;
                retVal = themeAdaptor.GetAssetAbsoluteUrl(input);
            }
            return retVal;
        }

        /// <summary>
        /// Returns the URL of a global assets that are found on Shopify's servers. 
        /// In virtocommerce is a same asset folder
        /// customer.css
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ShopifyAssetUrl(string input)
        {
            return AssetUrl(input);
        }


        /// <summary>
        /// Returns the URL of a global asset. Global assets are kept in a directory on Shopify's servers. Using global assets can improve the load times of your pages.
        /// In virtocommerce is a same asset folder
        // </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GlobalAssetUrl(string input)
        {
            return AssetUrl(input);
        }

        /// <summary>
        /// Returns the URL of a file.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FileUrl(string input)
        {
            return AssetUrl(input);
        }

        /// <summary>
        /// Url for <Base href=""/> html tag. Need add trailing "/" char
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string BaseUrl(string input)
        {
            var retVal = AbsoluteUrl(input);
            retVal += "/";
            return retVal;
        }
        /// <summary>
        /// Get absolute storefront url with specified store and language
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AbsoluteUrl(string input, string storeId = null, string languageCode = null)
        {
            var themeAdaptor = (ShopifyLiquidThemeEngine)Template.FileSystem;
            storefrontModel.Store store = null;
            storefrontModel.Language language = null;
            if (!string.IsNullOrEmpty(storeId))
            {
                store = themeAdaptor.WorkContext.AllStores.FirstOrDefault(x => string.Equals(x.Id, storeId, StringComparison.InvariantCultureIgnoreCase));
            }
            store = store ?? themeAdaptor.WorkContext.CurrentStore;

            if (!string.IsNullOrEmpty(languageCode))
            {
                language = store.Languages.FirstOrDefault(x => string.Equals(x.CultureName, languageCode, StringComparison.InvariantCultureIgnoreCase));
            }
            language = language ?? themeAdaptor.WorkContext.CurrentLanguage;

            var retVal = themeAdaptor.UrlBuilder.ToAppAbsolute(input, store, language);
            return retVal;
        }


        private static string BuildOnClickLink(string title, string onclickFormat, params object[] onclickArgs)
        {
            var onClick = string.Format(CultureInfo.InvariantCulture, onclickFormat, onclickArgs);

            var result = string.Format(CultureInfo.InvariantCulture, "<a href=\"#\" onclick=\"{0}\">{1}</a>",
               HttpUtility.HtmlAttributeEncode(onClick),
               HttpUtility.HtmlEncode(title));

            return result;
        }

        private static string BuildHtmlLink(string id, string virtualUrl, string title)
        {
            var href = BuildAbsoluteUrl(virtualUrl);

            var result = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" id=\"{1}\">{2}</a>",
               HttpUtility.HtmlAttributeEncode(href),
               HttpUtility.HtmlAttributeEncode(id),
               HttpUtility.HtmlEncode(title));

            return result;
        }


        private static string BuildAbsoluteUrl(string virtualUrl)
        {
            var themeEngine = (ShopifyLiquidThemeEngine)Template.FileSystem;
            var workContext = themeEngine.WorkContext;
            var result = themeEngine.UrlBuilder.ToAppAbsolute(virtualUrl, workContext.CurrentStore, workContext.CurrentLanguage);
            return result;
        }
    }
}
