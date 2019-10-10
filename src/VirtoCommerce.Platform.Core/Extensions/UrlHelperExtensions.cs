using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Core.Extensions
{
    /// <summary>
    /// <see cref="IUrlHelper"/> extension methods.
    /// </summary>
    public static partial class UrlHelperExtensions
    {
        private static string _host;
        private static string _scheme;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _host = httpContextAccessor.HttpContext.Request.Host.ToUriComponent();
            _scheme = httpContextAccessor.HttpContext.Request.Scheme;
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and
        /// route values.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteAction(
            this IUrlHelper urlHelper,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            return urlHelper.Action(actionName, controllerName, routeValues, urlHelper.ActionContext.HttpContext.Request.Scheme);
        }

        /// <summary>
        /// Generates a fully qualified URL to the specified content by using the specified content path. Converts a
        /// virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteContent(
            this IUrlHelper urlHelper,
            string contentPath)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            var request = urlHelper.ActionContext.HttpContext.Request;
            var uriBuilder = GetRootUriBuilder(request);
            uriBuilder.Path += urlHelper.Content(contentPath);

            return uriBuilder.Uri.ToString();
        }

        /// <summary>
        /// Generates a fully qualified URL to the specified route by using the route name and route values.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="publicPath"></param>
        /// <param name="urlPath">Name of the route.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteRouteUrl(
            this IUrlHelper urlHelper,
            string publicPath,
            string urlPath = null)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            var request = urlHelper.ActionContext.HttpContext.Request;

            var uriBuilder = GetRootUriBuilder(request, publicPath);

            if (!string.IsNullOrEmpty(urlPath))
            {
                uriBuilder.Path = Combine(uriBuilder.Path, urlPath.Replace(uriBuilder.Uri.ToString(), string.Empty));
            }

            return uriBuilder.Uri.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="publicPath"></param>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        public static string RelativeUrl(
            this IUrlHelper urlHelper,
            string publicPath,
            string urlPath)
        {
            var result = urlPath;

            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            var request = urlHelper.ActionContext.HttpContext.Request;

            var rootUrl = GetRootUriBuilder(request, publicPath).Uri.ToString();

            if (!string.IsNullOrEmpty(urlPath))
            {
                result = urlPath.Replace(rootUrl, string.Empty);
            }

            return result;
        }

        private static UriBuilder GetRootUriBuilder(HttpRequest request, string publicPath = null)
        {
            var result = new Uri($"{request.Scheme ?? _scheme}://{request.Host.Value ?? _host}");
            var uriBuilder = new UriBuilder(result);

            if (!string.IsNullOrEmpty(publicPath))
            {
                uriBuilder.Path += publicPath;
            }

            return uriBuilder;
        }

        public static string Combine(string baseUrl, string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return baseUrl;

            baseUrl = baseUrl.TrimEnd('/');
            relativeUrl = relativeUrl.TrimStart('/');

            return $"{baseUrl}/{relativeUrl}";
        }
    }
}
