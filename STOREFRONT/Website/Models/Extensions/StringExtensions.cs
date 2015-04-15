#region
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Extensions
{
    public static class StringExtensions
    {
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

        public static string ToAbsoluteUrl(this string relative)
        {
            if (relative == null) return null;
            //var urlHelper = GetUrlHelper();
            //urlHelper.RouteUrl()

            return relative.StartsWith("~") ? VirtualPathUtility.ToAbsolute(relative) : relative;
        }

        private static UrlHelper GetUrlHelper()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return null;
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);

            var urlHelper = new UrlHelper(requestContext);
            return urlHelper;

        }
        #endregion
    }
}