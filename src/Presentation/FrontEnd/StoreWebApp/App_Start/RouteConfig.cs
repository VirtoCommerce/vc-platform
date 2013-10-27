using System.Web.Mvc;
using System.Web.Routing;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class RouteConfig.
    /// </summary>
    public class RouteConfig
    {
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
                "FailWhale/{action}/{id}", new
                {
                    controller = "Error",
                    action = "FailWhale",
                    id = UrlParameter.Optional
                }
            );

			routes.MapRoute(
			   "Item_ln",
			   "{lang}/p/{url}",
			   new { controller = "Catalog", action = "DisplayItem" },
			   new { lang = "[a-z]{2}(-[A-Z]{2})?" });

            routes.MapRoute(
                "Item",
                "p/{url}",
                new { controller = "Catalog", action = "DisplayItem" }
            );

			routes.MapRoute(
			   "Catalog_ln",
			   "{lang}/c/{url}",
			   new { controller = "Catalog", action = "Display" },
			   new { lang = "[a-z]{2}(-[A-Z]{2})?" }
		   );

            routes.MapRoute(
                "Catalog",
                "c/{url}",
                new { controller = "Catalog", action = "Display" }
                //, new { IsRootAction = new IsRootActionConstraint() }  // Route Constraint
            );

            routes.MapRoute(
                "Assets",
                "asset/{*path}",
                new { controller = "Asset", action = "Index", path = UrlParameter.Optional }
            );

			routes.MapRoute(
			  "Localization", // Route name
			  "{lang}/{controller}/{action}/{id}", // URL with parameters
			  new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
			  new { lang = "[a-z]{2}(-[A-Z]{2})?" },
			  new[] { "VirtoCommerce.Web.Controllers" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "VirtoCommerce.Web.Controllers" }
            );
        }
    }
}