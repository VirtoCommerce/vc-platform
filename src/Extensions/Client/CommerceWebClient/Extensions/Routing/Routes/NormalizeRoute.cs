using System;
using System.Web;
using System.Web.Routing;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Routes
{
    public class NormalizeRoute : RouteBase
    {
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
            InternalRoute = route;
            AppendTrailingSlash = appendTrailingSlash;
            RequireLowerCase = requireLowerCase;
        }

        internal RouteBase InternalRoute
        {
            get;
            private set;
        }

        public bool AppendTrailingSlash
        {
            get;
            private set;
        }

        public bool RequireLowerCase
        {
            get;
            private set;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var internalRoute = InternalRoute as Route;
            if (internalRoute != null && !internalRoute.Url.Contains("{"))
            {
                return null;
            }


            //Some very specific code that maybe could be refactored later
            if (internalRoute is ItemRoute)
            {
                //For item route we need all params
            }
            else if (internalRoute is CategoryRoute)
            {
                //For category route we don't need item in query
                values.Remove("item");
            }
            else if (internalRoute is StoreRoute)
            {
                //For store route we don't need item and category in query
                values.Remove("item");
                values.Remove("category");
            }
            else
            {
                //For other routes we don't need any additional values
                values.Remove("item");
                values.Remove("category");
                values.Remove("store");
            }


            var vpd = InternalRoute.GetVirtualPath(requestContext, values);

            if (vpd != null)
            {
                var virtualPath = vpd.VirtualPath;
                if (RequireLowerCase)
                {
                    virtualPath = virtualPath.ToLowerInvariant();
                }

                var queryIndex = virtualPath.IndexOf('?');
                string queryPart = string.Empty;
                if (queryIndex > -1)
                {
                    queryPart = virtualPath.Substring(queryIndex);
                    virtualPath = virtualPath.Substring(0, queryIndex);
                }
                if (AppendTrailingSlash && !virtualPath.EndsWith("/"))
                {
                    virtualPath = virtualPath + "/";
                }

                if (!AppendTrailingSlash && virtualPath.EndsWith("/"))
                {
                    virtualPath = virtualPath.Substring(0, virtualPath.Length - 1);
                }
                virtualPath += queryPart;

                vpd.VirtualPath = virtualPath;
            }
            return vpd;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return InternalRoute.GetRouteData(httpContext);
        }
    }
}