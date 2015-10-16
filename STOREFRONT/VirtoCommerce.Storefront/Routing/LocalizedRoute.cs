using System.Web;
using System.Web.Routing;

namespace VirtoCommerce.Storefront.Routing
{
    public class LocalizedRoute : Route
    {
        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern and handler class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public LocalizedRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath;
            string applicationPath = httpContext.Request.ApplicationPath;
            if (virtualPath.IsLocalizedUrl(applicationPath, false))
            {
                //In ASP.NET Development Server, an URL like "http://localhost/Blog.aspx/Categories/BabyFrog" will return 
                //"~/Blog.aspx/Categories/BabyFrog" as AppRelativeCurrentExecutionFilePath.
                //However, in II6, the AppRelativeCurrentExecutionFilePath is "~/Blog.aspx"
                //It seems that IIS6 think we're process Blog.aspx page.
                //So, I'll use RawUrl to re-create an AppRelativeCurrentExecutionFilePath like ASP.NET Development Server.

                //Question: should we do path rewriting right here?
                string rawUrl = httpContext.Request.RawUrl;
                var newVirtualPath = rawUrl.RemoveLanguageSeoCodeFromRawUrl(applicationPath);
                if (string.IsNullOrEmpty(newVirtualPath))
                    newVirtualPath = "/";
                newVirtualPath = newVirtualPath.RemoveApplicationPathFromRawUrl(applicationPath);
                newVirtualPath = "~" + newVirtualPath;
                httpContext.RewritePath(newVirtualPath, true);
            }

            var data = base.GetRouteData(httpContext);
            return data;
        }

        /// <summary>
        /// Returns information about the URL that is associated with the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains information about the URL that is associated with the route.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var data = base.GetVirtualPath(requestContext, values);

            if (data != null)
            {
                string rawUrl = requestContext.HttpContext.Request.RawUrl;
                string applicationPath = requestContext.HttpContext.Request.ApplicationPath;
                if (rawUrl.IsLocalizedUrl(applicationPath, true))
                {
                    data.VirtualPath = string.Concat(rawUrl.GetLanguageSeoCodeFromUrl(applicationPath, true), "/",
                        data.VirtualPath);
                }
            }

            return data;
        }
    }
}
