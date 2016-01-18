using System;
using System.Web.Mvc;
using System.Web.Routing;
using CacheManager.Core;
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
            routes.MapLocalizedStorefrontRoute(name, url, defaults, defaults, x => new Route(x, new MvcRouteHandler()));
        }

        /// <summary>
        /// Generate extra three routing for specified url {store}/url, {store}/{language}/url, {language}/url, url
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="routeFactory"></param>
        public static void MapLocalizedStorefrontRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, Func<string, Route> routeFactory)
        {
            var languageConstrain =  @"[a-z]{2}-[A-Z]{2}";

            var languageWithStoreRoute = routeFactory(@"{store}/{language}/" + url);
            languageWithStoreRoute.Defaults = new RouteValueDictionary(defaults);
            languageWithStoreRoute.Constraints = new RouteValueDictionary(constraints);
            languageWithStoreRoute.DataTokens = new RouteValueDictionary();
            languageWithStoreRoute.Constraints.Add("language", languageConstrain);
            routes.Add(name + "StoreWithLang", languageWithStoreRoute);

            var languageRoute = routeFactory(@"{language}/" + url);
            languageRoute.Defaults = new RouteValueDictionary(defaults);
            languageRoute.Constraints = new RouteValueDictionary(constraints);
            languageRoute.DataTokens = new RouteValueDictionary();

            languageRoute.Constraints.Add("language", languageConstrain);
            routes.Add(name + "Lang", languageRoute);

            var storeRoute = routeFactory(@"{store}/" + url);
            storeRoute.Defaults = new RouteValueDictionary(defaults);
            storeRoute.Constraints = new RouteValueDictionary(constraints);
            storeRoute.DataTokens = new RouteValueDictionary();
            routes.Add(name + "Store", storeRoute);

            var route = routeFactory(url);
            route.Defaults = new RouteValueDictionary(defaults);
            route.Constraints = new RouteValueDictionary(constraints);
            route.DataTokens = new RouteValueDictionary();
            routes.Add(name, route);
        }
        
    }
}
