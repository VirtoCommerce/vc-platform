using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new CheckPermissionAttribute { Permission = PredefinedPermissions.SecurityCallApi });

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
            config.AddODataQueryFilter();

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.Formatting = Formatting.Indented;
            json.SerializerSettings.Error += (sender, args) =>
            {
                //Need escalate any JSON serialization exception as HTTP error
                HttpContext.Current.AddError(args.ErrorContext.Error);
            };
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
