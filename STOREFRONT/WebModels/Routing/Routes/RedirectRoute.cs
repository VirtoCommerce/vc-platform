#region
using System;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Web.Models.Extensions;
using VirtoCommerce.Web.Models.Routing.HttpHandlers;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Routes
{
    // Craziness! In this case, there's no reason the route can't be its own route handler.
    public class RedirectRoute : RouteBase, IRouteHandler
    {
        #region Constructors and Destructors
        public RedirectRoute(RouteBase sourceRoute, RouteBase targetRoute, bool permanent)
            : this(sourceRoute, targetRoute, permanent, null)
        {
        }

        public RedirectRoute(
            RouteBase sourceRoute,
            RouteBase targetRoute,
            bool permanent,
            Func<RequestContext, RouteValueDictionary> additionalRouteValues)
        {
            this.SourceRoute = sourceRoute;
            this.TargetRoute = targetRoute;
            this.Permanent = permanent;
            this.AdditionalRouteValuesFunc = additionalRouteValues;
        }
        #endregion

        #region Public Properties
        public Func<RequestContext, RouteValueDictionary> AdditionalRouteValuesFunc { get; private set; }

        public bool Permanent { get; set; }

        public RouteBase SourceRoute { get; set; }

        public RouteBase TargetRoute { get; set; }
        #endregion

        #region Public Methods and Operators
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var requestRouteValues = requestContext.RouteData.Values;

            var routeValues = requestRouteValues.Merge(this.AdditionalRouteValuesFunc(requestContext));

            var vpd = this.TargetRoute.GetVirtualPath(requestContext, routeValues);
            if (vpd != null)
            {
                var targetUrl = "~/" + vpd.VirtualPath;
                return new RedirectHttpHandler(targetUrl, this.Permanent, false);
            }
            return new DelegateHttpHandler(
                rc => rc.HttpContext.Response.StatusCode = 404,
                requestContext.RouteData,
                false);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Use the original route to match
            var routeData = this.SourceRoute.GetRouteData(httpContext);
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
            return this.To(targetRoute, x => null);
        }

        public RedirectRoute To(RouteBase targetRoute, object routeValues)
        {
            return this.To(targetRoute, new RouteValueDictionary(routeValues));
        }

        public RedirectRoute To(RouteBase targetRoute, RouteValueDictionary routeValues)
        {
            return this.To(targetRoute, x => routeValues);
        }

        public RedirectRoute To(RouteBase targetRoute, Func<RequestContext, RouteValueDictionary> routeValues)
        {
            if (targetRoute == null)
            {
                throw new ArgumentNullException("targetRoute");
            }

            // Set once only
            if (this.TargetRoute != null)
            {
                throw new InvalidOperationException( /* TODO */);
            }
            this.TargetRoute = targetRoute;

            // Set once only
            if (this.AdditionalRouteValuesFunc != null)
            {
                throw new InvalidOperationException( /* TODO */);
            }
            this.AdditionalRouteValuesFunc = routeValues;
            return this;
        }
        #endregion
    }
}