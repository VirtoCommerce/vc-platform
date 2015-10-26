using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Routing;

namespace VirtoCommerce.Storefront
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, ICommerceCoreModuleApi commerceCoreApi)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapSeoRoute(commerceCoreApi, "SeoRoute", "{seo_slug}", new { controller = "Common", action = "GenericUrl" });

            routes.MapRoute(
              name: "Storefront_Error",
              url: "Error/{code}",
              defaults: new { controller = "Error", action = "Index", code = 500 });

            routes.MapRoute(
                name: "Storefront_Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
