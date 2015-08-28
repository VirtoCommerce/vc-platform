using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models.Routing;

namespace VirtoCommerce.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapSeoRoutes(); // maps seo defined on product, category and store levels in virto commerce

            routes.MapRoute(
              name: "Storefront_Error",
              url: "Error/{code}",
              defaults: new { controller = "Error", action = "Index", code = 500 });

            routes.MapRoute(
                name: "Storefront_Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
