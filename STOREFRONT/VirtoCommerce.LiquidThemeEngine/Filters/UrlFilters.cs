using System;
using System.Globalization;
using System.Linq;
using System.Web;
using DotLiquid;
using VirtoCommerce.Storefront.Model;

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
            return BuildOnClickLink(input, "Shopify.CustomerAddress.toggleForm({0});return false", id);
        }

        public static string DeleteCustomerAddressLink(string input, string id)
        {
            return BuildOnClickLink(input, "Shopify.CustomerAddress.destroy({0});return false", id);
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
        /// Get absolute storefront url with specified store and language
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AbsoluteUrl(string input, string storeId = null, string languageCode = null)
        {
            var themeAdaptor = (ShopifyLiquidThemeEngine)Template.FileSystem;
            Store store = null;
            Language language = null;
            if (!string.IsNullOrEmpty(storeId))
            {
                store = themeAdaptor.WorkContext.AllStores.FirstOrDefault(x => string.Equals(x.Id, storeId, StringComparison.InvariantCultureIgnoreCase));
            }
            store = store ?? themeAdaptor.WorkContext.CurrentStore;

            if (!string.IsNullOrEmpty(languageCode))
            {
                language = store.Languages.FirstOrDefault(x => string.Equals(x.CultureName, languageCode, StringComparison.InvariantCultureIgnoreCase));
            }
            language = language ?? store.DefaultLanguage;

            var retVal = themeAdaptor.UrlBuilder.ToAppAbsolute(themeAdaptor.WorkContext, input, store, language);
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
            var result = themeEngine.UrlBuilder.ToAppAbsolute(workContext, virtualUrl, workContext.CurrentStore, workContext.CurrentLanguage);
            return result;
        }
    }
}
