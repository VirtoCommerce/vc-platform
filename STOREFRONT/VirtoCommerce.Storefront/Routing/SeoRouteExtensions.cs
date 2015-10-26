using System;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Client.Api;

namespace VirtoCommerce.Storefront.Routing
{
    public static class SeoRouteExtensions
    {
        public static Route MapSeoRoute(this RouteCollection routes, ICommerceCoreModuleApi commerceCoreApi, string name, string url, object defaults)
        {
            return MapSeoRoute(routes, commerceCoreApi, name, url, defaults, null, null);
        }

        public static Route MapSeoRoute(this RouteCollection routes, ICommerceCoreModuleApi commerceCoreApi, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var route = new SeoRoute(url, new MvcRouteHandler(), commerceCoreApi)
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }
    }
}
