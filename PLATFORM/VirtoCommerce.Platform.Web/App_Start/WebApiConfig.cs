using System.Web.Http;
using System.Web.Http.OData.Extensions;
using Newtonsoft.Json;

using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new CheckPermissionAttribute { Permission = "security:call_api" });

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            //config.Routes.MapHttpRoute("DefaultApiGet", "api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"^[0-9]+$" });
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional, action = RouteParameter.Optional });

            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new DeltaContractResolver(formatter);
            formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            formatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //formatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            //config.Formatters.JsonFormatter.SerializerSettings.
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();
            config.AddODataQueryFilter();

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
