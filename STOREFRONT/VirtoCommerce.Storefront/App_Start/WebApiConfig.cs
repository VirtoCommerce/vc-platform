using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Storefront
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            formatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //formatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            formatter.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            //config.Formatters.JsonFormatter.SerializerSettings.
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
