using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// Allows to ignore <see cref="Newtonsoft.Json.JsonIgnoreAttribute"/>.
    /// </summary>
    public class NewtonsoftJsonIgnoreFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                     .Where(p => p.GetCustomAttributes(typeof(Newtonsoft.Json.JsonIgnoreAttribute), true)?.Any() == true))
            {
                var propName = prop.Name[0].ToString().ToLower() + prop.Name.Substring(1);
                if (schema?.Properties?.ContainsKey(propName) == true)
                    schema?.Properties?.Remove(propName);
            }
        }
    }
}
