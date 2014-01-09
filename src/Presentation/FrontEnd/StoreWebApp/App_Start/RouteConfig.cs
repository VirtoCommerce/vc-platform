using System.Collections.Generic;
using System.Configuration;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;
using VirtoCommerce.Web.Client.Extensions.Routing;

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

            routes.MapRoute("Store", "{lang}/{store}",
               new { controller = "Home", action = "Index" },
               new { lang = "[a-z]{2}(-[A-Z]{2})?" });


            routes.Add("Item", new ItemRoute("{lang}/{store}/{category}/{item}",
                CreateRouteValueDictionary(new { controller = "Catalog", action = "DisplayItem", store = ConfigurationManager.AppSettings["DefaultStore"] }),
                CreateRouteValueDictionary(new { lang = "[a-z]{2}(-[A-Z]{2})?" }),
                new ItemRouteHandler()));

            #region Legacy routes for compatibility

            routes.MapRoute(
              "Item_legacy",
              "p/{item}",
              new { controller = "Catalog", action = "DisplayItem" });

            routes.MapRoute(
                "Item_legacy_ln",
                "{lang}/p/{item}",
                new { controller = "Catalog", action = "DisplayItem" },
                new { lang = "[a-z]{2}(-[A-Z]{2})?" });

            #endregion

            routes.Add("Catalog", new CategoryRoute("{lang}/{store}/{*category}",
                CreateRouteValueDictionary(new { controller = "Catalog", action = "Display", store = ConfigurationManager.AppSettings["DefaultStore"] }),
                CreateRouteValueDictionary(new { lang = "[a-z]{2}(-[A-Z]{2})?" }),
                new CategoryRouteHandler()));

            routes.MapRoute(
              "Localization", // Route name
              "{lang}/{controller}/{action}/{id}", // URL with parameters
              new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
              new { lang = "[a-z]{2}(-[A-Z]{2})?" },
              new[] { "VirtoCommerce.Web.Controllers" });
        }
    }
}