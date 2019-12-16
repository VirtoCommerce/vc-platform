using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.OData.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Platform.Web.App_Start;

namespace VirtoCommerce.Platform.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new CheckPermissionAttribute { Permission = PredefinedPermissions.SecurityCallApi });

            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var jsonFormatter = config.Formatters.JsonFormatter;

            //Next line needs to represent custom derived types in the resulting swagger doc definitions. Because default SwaggerProvider used global JSON serialization settings
            //we should register this converter globally.
            jsonFormatter.SerializerSettings.ContractResolver = new PolymorphJsonContractResolver();

            jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            jsonFormatter.SerializerSettings.Formatting = Formatting.None;
            jsonFormatter.MediaTypeMappings.Add(new RequestHeaderMapping("Accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, "application/json"));

            jsonFormatter.SerializerSettings.Error += (sender, args) =>
            {
                // Expose any JSON serialization exception as HTTP error
                HttpContext.Current.AddError(args.ErrorContext.Error);
            };

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();
            config.AddODataQueryFilter();

            config.Services.Replace(typeof(IExceptionHandler), new AiExceptionHandler());
        }

        /// <summary>
        /// Used for represent derived (overridden) types in resulting swagger API docs.
        /// This converter gets derived types from AbstractTypeFactory
        /// </summary>
        public class PolymorphJsonContractResolver : CamelCasePropertyNamesContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                //Do not handle abstract types
                if (!type.IsAbstract)
                {
                    var abstractTypeFactory = typeof(Core.Common.AbstractTypeFactory<>).MakeGenericType(type);
                    var pi = abstractTypeFactory.GetProperty("AllTypeInfos");
                    var values = pi.GetValue(null) as IList;
                    //Handle only types which have any registered derived type
                    if (values.Count > 0)
                    {
                        type = abstractTypeFactory.GetMethods().Where(x => x.Name == "TryCreateInstance").First().Invoke(null, null).GetType();
                    }
                }
                return base.CreateProperties(type, memberSerialization);
            }
        }

    }
}
