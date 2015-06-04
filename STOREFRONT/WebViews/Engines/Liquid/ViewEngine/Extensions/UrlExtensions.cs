#region
using System;
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Extensions
{

    #region
    #endregion

    public static class UrlExtensions
    {
        #region Public Methods and Operators
        public static string ContentAbsolute(this UrlHelper url, string contentPath)
        {
            return new Uri(GetBaseUrl(url), url.Content(contentPath)).AbsoluteUri;
        }

        public static Uri GetBaseUrl(this UrlHelper url)
        {
            var uri = new Uri(url.RequestContext.HttpContext.Request.Url, url.RequestContext.HttpContext.Request.RawUrl);
            var builder = new UriBuilder(uri)
                          {
                              Path = url.RequestContext.HttpContext.Request.ApplicationPath,
                              Query = null,
                              Fragment = null
                          };
            return builder.Uri;
        }
        #endregion
    }
}