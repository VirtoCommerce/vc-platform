#region
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models.Routing;

#endregion

namespace VirtoCommerce.Web
{
    public class RouteConfig
    {
        #region Public Methods and Operators
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapSeoRoutes(); // maps seo defined on product, category and store levels in virto commerce

            routes.MapRoute(
                name: "404 Not Found",
                url: "404",
                defaults: new { controller = "Errors", action = "NotFound" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults:new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
        #endregion
    }
}