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

        public RedirectRoute(RouteBase sourceRoute, RouteBase targetRoute, bool permanent, Func<RequestContext, RouteValueDictionary> additionalRouteValues)
        {
            SourceRoute = sourceRoute;
            TargetRoute = targetRoute;
            Permanent = permanent;
            AdditionalRouteValuesFunc = additionalRouteValues;
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

        public Func<RequestContext, RouteValueDictionary> AdditionalRouteValuesFunc
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
            return To(targetRoute, x => null);
        }

        public RedirectRoute To(RouteBase targetRoute, object routeValues)
        {
            return To(targetRoute, new RouteValueDictionary(routeValues));
        }

        public RedirectRoute To(RouteBase targetRoute, RouteValueDictionary routeValues)
        {
            return To(targetRoute, x => routeValues);
        }

        public RedirectRoute To(RouteBase targetRoute, Func<RequestContext, RouteValueDictionary> routeValues)
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
            if (AdditionalRouteValuesFunc != null)
            {
                throw new InvalidOperationException(/* TODO */);
            }
            AdditionalRouteValuesFunc = routeValues;
            return this;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var requestRouteValues = requestContext.RouteData.Values;

            var routeValues = requestRouteValues.Merge(AdditionalRouteValuesFunc(requestContext));

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
