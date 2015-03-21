#region
using System;
using System.Web;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Routes
{
    public class NormalizeRoute : RouteBase
    {
        #region Constructors and Destructors
        public NormalizeRoute(RouteBase route)
            : this(route, requireLowerCase: true, appendTrailingSlash: false)
        {
        }

        public NormalizeRoute(RouteBase route, bool requireLowerCase, bool appendTrailingSlash)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }
            this.InternalRoute = route;
            this.AppendTrailingSlash = appendTrailingSlash;
            this.RequireLowerCase = requireLowerCase;
        }
        #endregion

        #region Public Properties
        public bool AppendTrailingSlash { get; private set; }

        public bool RequireLowerCase { get; private set; }
        #endregion

        #region Properties
        internal RouteBase InternalRoute { get; private set; }
        #endregion

        #region Public Methods and Operators
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return this.InternalRoute.GetRouteData(httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var internalRoute = this.InternalRoute as Route;
            if (internalRoute != null && !internalRoute.Url.Contains("{"))
            {
                return null;
            }

            //TODO: Some very specific code that should be refactored later
            if (internalRoute is ItemRoute)
            {
                //For item route we need all params
            }
            else if (internalRoute is CategoryRoute)
            {
                //For category route we don't need item in query
                values.Remove(Constants.Item);
            }
            else if (internalRoute is StoreRoute)
            {
                //For store route we don't need item and category in query
                values.Remove(Constants.Item);
                values.Remove(Constants.Category);
            }
            else
            {
                //For other routes we don't need any additional values
                values.Remove(Constants.Item);
                values.Remove(Constants.Category);
                values.Remove(Constants.Store);
            }

            var vpd = this.InternalRoute.GetVirtualPath(requestContext, values);

            if (vpd != null)
            {
                var virtualPath = vpd.VirtualPath;

                var queryIndex = virtualPath.IndexOf('?');
                var queryPart = string.Empty;
                if (queryIndex > -1)
                {
                    queryPart = virtualPath.Substring(queryIndex);
                    virtualPath = virtualPath.Substring(0, queryIndex);
                }
                if (this.RequireLowerCase)
                {
                    virtualPath = virtualPath.ToLowerInvariant();
                }
                if (this.AppendTrailingSlash && !virtualPath.EndsWith("/"))
                {
                    virtualPath = virtualPath + "/";
                }

                if (!this.AppendTrailingSlash && virtualPath.EndsWith("/"))
                {
                    virtualPath = virtualPath.Substring(0, virtualPath.Length - 1);
                }
                virtualPath += queryPart;

                //Decode virtualPath to show nicely cyrillic url
                vpd.VirtualPath = HttpUtility.UrlDecode(virtualPath);
            }
            return vpd;
        }
        #endregion
    }
}