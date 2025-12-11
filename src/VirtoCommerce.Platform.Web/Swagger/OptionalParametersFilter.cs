using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class OptionalParametersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null || !operation.Parameters.Any())
            {
                return;
            }

            var optionalParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ParameterDescriptor is ControllerParameterDescriptor controllerParamDescriptor
                            && controllerParamDescriptor.ParameterInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(SwaggerOptionalAttribute)))
                .ToList();

            for (int i = 0; i < operation.Parameters.Count; i++)
            {
                var parameter = operation.Parameters[i];
                var apiParameter = optionalParameters.FirstOrDefault(p => p.Name == parameter.Name);
                if (apiParameter != null)
                {
                    // Create a new parameter with Required = false since Required is read-only in OpenAPI v3
                    var newParameter = new OpenApiParameter
                    {
                        Name = parameter.Name,
                        In = parameter.In,
                        Description = parameter.Description,
                        Required = false,
                        Deprecated = parameter.Deprecated,
                        AllowEmptyValue = parameter.AllowEmptyValue,
                        Style = parameter.Style,
                        Explode = parameter.Explode,
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
