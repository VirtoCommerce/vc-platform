using System;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Routing;

namespace VirtoCommerce.Storefront
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapSeoRoute(workContextFactory, commerceCoreApi, "SeoRoute", "{*path}", new { controller = "Common", action = "GenericUrl" });
        }
    }
}
