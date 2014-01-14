using System;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.Routing.HttpHandlers;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Routes
{
    // Craziness! In this case, there's no reason the route can't be its own route handler.
    public class RedirectRoute : RouteBase, IRouteHandler
    {
        public RedirectRoute(RouteBase sourceRoute, RouteBase targetRoute, bool permanent)
            : this(sourceRoute, targetRoute, permanent, null)
        {
        }

        public RedirectRoute(RouteBase sourceRoute, RouteBase targetRoute, bool permanent, RouteValueDictionary additionalRouteValues)
        {
            SourceRoute = sourceRoute;
            TargetRoute = targetRoute;
            Permanent = permanent;
            AdditionalRouteValues = additionalRouteValues;
        }

        public RouteBase SourceRoute
        {
            get;
            set;
        }

        public RouteBase TargetRoute
        {
            get;
            set;
        }

        public bool Permanent
        {
            get;
            set;
        }

        public RouteValueDictionary AdditionalRouteValues
        {
            get;
            private set;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Use the original route to match
            var routeData = SourceRoute.GetRouteData(httpContext);
            if (routeData == null)
            {
                return null;
            }
            // But swap its route handler with our own
            routeData.RouteHandler = this;
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Redirect routes never generate an URL.
            return null;
        }

        public RedirectRoute To(RouteBase targetRoute)
        {
            return To(targetRoute, null);
        }

        public RedirectRoute To(RouteBase targetRoute, object routeValues)
        {
            return To(targetRoute, new RouteValueDictionary(routeValues));
        }

        public RedirectRoute To(RouteBase targetRoute, RouteValueDictionary routeValues)
        {
            if (targetRoute == null)
            {
                throw new ArgumentNullException("targetRoute");
            }

            // Set once only
            if (TargetRoute != null)
            {
                throw new InvalidOperationException(/* TODO */);
            }
            TargetRoute = targetRoute;

            // Set once only
            if (AdditionalRouteValues != null)
            {
                throw new InvalidOperationException(/* TODO */);
            }
            AdditionalRouteValues = routeValues;
            return this;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var requestRouteValues = requestContext.RouteData.Values;

            var routeValues = AdditionalRouteValues.Merge(requestRouteValues);

            var vpd = TargetRoute.GetVirtualPath(requestContext, routeValues);
            if (vpd != null)
            {
                string targetUrl = "~/" + vpd.VirtualPath;
                return new RedirectHttpHandler(targetUrl, Permanent, false);
            }
            return new DelegateHttpHandler(rc => rc.HttpContext.Response.StatusCode = 404, requestContext.RouteData, false);
        }

    }
}
