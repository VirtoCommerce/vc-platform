using System;
using System.Web.Mvc;
using System.Web.Routing;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Routing
{
    public static class StorefrontRouteExtensions
    {
        public static void AddStorefrontRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            routes.AddStorefrontRoute(name, url, defaults, null);
        }

        public static void AddStorefrontRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            routes.AddStorefrontRoute(name, url, defaults, constraints, x => new Route(x, new MvcRouteHandler()));
        }

        /// <summary>
        /// Register storefront route with follow scheme {store}/{language}/url
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="routeFactory"></param>
        public static void AddStorefrontRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, Func<string, Route> routeFactory)
        {
            var languageConstrain = @"[a-z]{2}-[a-zA-Z]{2}";

            var languageWithStoreRoute = routeFactory(@"{store}/{language}/" + url);
            languageWithStoreRoute.Defaults = new RouteValueDictionary(defaults);
            languageWithStoreRoute.Constraints = new RouteValueDictionary(constraints);
            languageWithStoreRoute.DataTokens = new RouteValueDictionary();
            languageWithStoreRoute.Constraints.Add("language", languageConstrain);
            routes.Add(name + "StoreWithLang", languageWithStoreRoute);
        }
        
    }
}
