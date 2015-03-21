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
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
        #endregion
    }
}