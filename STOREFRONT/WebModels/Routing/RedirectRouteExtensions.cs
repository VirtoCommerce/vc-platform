using System;
using System.Web.Routing;
using VirtoCommerce.Web.Models.Routing.Routes;

namespace VirtoCommerce.Web.Models.Routing
{
    public static class RedirectRouteExtensions
    {
        // We always want to map the RedirectRoute *BEFORE* the legacy route that we're going to redirect.
        // Otherwise the redirect route will never match because the legacy route will supersede it. 
        // Hence the Func<RouteCollection, RouteBase>.
        public static RedirectRoute Redirect(this RouteCollection routes, Func<RouteCollection, RouteBase> routeMapping, bool permanent = false)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (routeMapping == null)
            {
                throw new ArgumentNullException("routeMapping");
            }

            var routeCollection = new RouteCollection();
            var legacyRoute = routeMapping(routeCollection);

            var redirectRoute = new RedirectRoute(legacyRoute, null, permanent, null);
            routes.Add(new NormalizeRoute(redirectRoute));
            return redirectRoute;
        }
    }
}