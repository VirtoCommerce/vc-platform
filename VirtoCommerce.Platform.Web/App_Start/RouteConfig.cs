﻿using System.Web.Mvc;
using System.Web.Routing;

namespace VirtoCommerce.Platform.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            if (Startup.IsApplication)
            {
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    namespaces: new[] { "VirtoCommerce.Platform.Web.Controllers" }
                    );
            }
        }
    }
}
