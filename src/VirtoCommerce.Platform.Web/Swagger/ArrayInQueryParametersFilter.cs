using System.Linq;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// The workaround for openapi3/autorest query parameters serialization
    /// See more: https://github.com/Azure/autorest/issues/3373, https://swagger.io/specification/
    /// </summary>
    public class ArrayInQueryParametersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            for (int i = 0; i < operation.Parameters.Count; i++)
            {
                var parameter = operation.Parameters[i];
                if (parameter.In == ParameterLocation.Query
                    && parameter.Schema.Type == JsonSchemaType.Array
                    && !parameter.Style.HasValue)
                {
                    // Create a new parameter with the required Style and Explode properties
                    // since these properties are read-only in OpenAPI v3
                    var newParameter = new OpenApiParameter
                    {
                        Name = parameter.Name,
                        In = parameter.In,
                        Description = parameter.Description,
                        Required = parameter.Required,
                        Deprecated = parameter.Deprecated,
                        AllowEmptyValue = parameter.AllowEmptyValue,
                        Style = ParameterStyle.Form,
                        Explode = true,
                        AllowReserved = parameter.AllowReserved,
                        Schema = parameter.Schema,
                        Example = parameter.Example,
                        Examples = parameter.Examples,
                        Content = parameter.Content
                    };
                    operation.Parameters[i] = newParameter;
                }
            }
        }
    }
}
