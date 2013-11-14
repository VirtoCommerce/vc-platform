using System.Web.Http;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the api controller routes.
        /// </summary>
        /// <param name="config">The http configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional, action = RouteParameter.Optional });
        }
    }
}
