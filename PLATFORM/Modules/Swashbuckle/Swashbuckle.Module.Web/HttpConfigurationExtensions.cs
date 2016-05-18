using System;
using System.Web.Http;
using Swashbuckle.Application;

namespace SwashbuckleModule.Web
{
    public static class HttpConfigurationExtensions
    {
        public static SwaggerEnabledConfiguration EnableSwagger(this HttpConfiguration httpConfig, string routeName, string routeTemplate, Action<SwaggerDocsConfig> configure = null)
        {
            var config = new SwaggerDocsConfig();
            if (configure != null)
            {
                configure(config);
            }
            httpConfig.Routes.MapHttpRoute(routeName, routeTemplate, null, new { apiVersion = ".+" }, new SwaggerDocsHandler(config));
            return new SwaggerEnabledConfiguration(httpConfig, null, null);
        }
    }
}
