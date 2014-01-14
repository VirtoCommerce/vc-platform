using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Extensions.Routing.Routes;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class RouteConfig.
    /// </summary>
    public class RouteConfig
    {

        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            var dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new RouteValueDictionary(dictionary);
            }

            return new RouteValueDictionary(values);
        }
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("virto/services/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("virto/dataservices/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute(".html");

            routes.MapRoute(
                "FailWhale",
                "FailWhale/{action}/{id}", new { controller = "Error", action = "FailWhale", id = UrlParameter.Optional });

            routes.MapRoute(
              "Assets",
              "asset/{*path}",
              new { controller = "Asset", action = "Index", path = UrlParameter.Optional }
          );

            var itemRoute = new NormalizeRoute(new ItemRoute("{lang}/{store}/{category}/{item}",
                CreateRouteValueDictionary(
                    new
                    {
                        controller = "Catalog",
                        action = "DisplayItem",
                        store = ConfigurationManager.AppSettings["DefaultStore"]
                    }),
                CreateRouteValueDictionary(new {lang = "[a-z]{2}(-[A-Z]{2})?"}),
                new MvcRouteHandler())
                {
                    DataTokens = new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } }
                });

            var categoryRoute = new NormalizeRoute(new CategoryRoute("{lang}/{store}/{category}",
                CreateRouteValueDictionary(
                    new
                    {
                        controller = "Catalog",
                        action = "Display",
                        store = ConfigurationManager.AppSettings["DefaultStore"]
                    }),
                CreateRouteValueDictionary(new {lang = "[a-z]{2}(-[A-Z]{2})?"}),
                new MvcRouteHandler())
                {
                    DataTokens = new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } }
                });

            var storeRoute = new NormalizeRoute(new StoreRoute("{lang}/{store}",
                CreateRouteValueDictionary(new {controller = "Home", action = "Index"}),
                CreateRouteValueDictionary(new {lang = "[a-z]{2}(-[A-Z]{2})?"}),
                new MvcRouteHandler())
                {
                    DataTokens = new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } }
                });

            routes.Add("Item", itemRoute);
            routes.Add("Catalog", categoryRoute);
            routes.Add("Store", storeRoute);

            //Legacy redirects
            routes.Redirect(r => r.MapRoute("old_Category", "c/{category}")).To(categoryRoute);
            routes.Redirect(r => r.MapRoute("old_Item", "p/{item}")).To(itemRoute);

            var otherRoute = new NormalizeRoute(new Route("{lang}/{controller}/{action}/{id}",
                CreateRouteValueDictionary(new { id = UrlParameter.Optional }),
                CreateRouteValueDictionary(new { lang = "[a-z]{2}(-[A-Z]{2})?" }), new MvcRouteHandler())
            {
                DataTokens = new RouteValueDictionary { { "namespaces", new[] { "VirtoCommerce.Web.Controllers" } } }
            });

            //Other actions
            routes.Add("Default", otherRoute);

            //Needed for some post requests
            routes.MapRoute(
                "Default_Fallback", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    //controller = "Home", 
                    //action = "Index", 
                    id = UrlParameter.Optional
                }, // Parameter defaults
                new[] { "VirtoCommerce.Web.Controllers" });
        }
    }
}