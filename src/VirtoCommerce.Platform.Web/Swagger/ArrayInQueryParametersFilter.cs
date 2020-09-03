using System.Linq;
using Microsoft.OpenApi.Models;
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
            foreach (var parameter in operation.Parameters.Where(x => (x.In == ParameterLocation.Query && x.Schema.Type == "array")))
            {
                if (!parameter.Style.HasValue)
                {
                    parameter.Style = ParameterStyle.Form;
                    parameter.Explode = true;
                }
            }
        }
    }
}
