#region

using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Extensions
{
    public static class StringExtensions
    {
        public static string TrimStart(this string target, string trimString)
        {
            var result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        #region Public Methods and Operators
        public static CultureInfo TryGetCultureInfo(this string languageCode)
        {
            try
            {
                return !string.IsNullOrEmpty(languageCode) ? CultureInfo.CreateSpecificCulture(languageCode) : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 || (url.Scheme == "https" && url.Port == 443) ? (":" + url.Port) : String.Empty;

            return string.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        public static bool IsActiveUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return false;

            if (HttpContext.Current == null)
                return false;

            if (relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.TrimStart("~");

            var url = HttpContext.Current.Request.Url;

            if (url.PathAndQuery.StartsWith(relativeUrl) && relativeUrl != "/")
                return true;

            return false;
        }
        #endregion
    }
}