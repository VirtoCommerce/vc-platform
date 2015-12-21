using System;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Routing
{
    public static class SeoRouteExtensions
    {
        public static void MapLocalizedStorefrontRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            routes.MapLocalizedStorefrontRoute(name, url, defaults, null);
        }
        public static void MapLocalizedStorefrontRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            var languageConstrain =  @"[a-z]{2}-[A-Z]{2}";
      
            var languageWithStoreRoute = new Route(@"{store}/{language}/" + url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            languageWithStoreRoute.Constraints.Add("language", languageConstrain);
            routes.Add(name + "StoreWithLang", languageWithStoreRoute);

            var languageRoute = new Route(@"{language}/" + url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            languageRoute.Constraints.Add("language", languageConstrain);
            routes.Add(name + "Lang", languageRoute);

            var storeRoute = new Route(@"{store}/" + url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            routes.Add(name + "Store", storeRoute);

            var route = new Route(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            routes.Add(name, route);
        }

        public static Route MapSeoRoute(this RouteCollection routes, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, IStaticContentService staticContentService, string name, string url, object defaults)
        {
            return MapSeoRoute(routes, workContextFactory, commerceCoreApi, staticContentService, name, url, defaults, null, null);
        }

        public static Route MapSeoRoute(this RouteCollection routes, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, IStaticContentService staticContentService,   string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var route = new SeoRoute(url, new MvcRouteHandler(), workContextFactory, commerceCoreApi, staticContentService)
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
