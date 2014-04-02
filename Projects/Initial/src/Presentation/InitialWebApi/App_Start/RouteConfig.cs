using System.Web.Mvc;
using System.Web.Routing;

namespace Initial.WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("virto/services/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("virto/dataservices/{MyJob}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute(".html");

            //Needed for some post requests
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    action = "Index",
                    controller="Home",
                    id = UrlParameter.Optional
                }, // Parameter defaults
                new[] { "Initial.WebApi.Controllers" });
        }
    }
}