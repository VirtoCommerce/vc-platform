using System;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class OptionalParametersFilter : IOperationFilter
    {
        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var optionalParameters = apiDescription.ParameterDescriptions
                .Where(p => p.ParameterDescriptor != null && p.ParameterDescriptor.GetCustomAttributes<SwaggerOptionalAttribute>().Any())
                .ToList();

            foreach (var apiParameter in optionalParameters)
            {
                var parameter = operation.parameters.FirstOrDefault(p => p.name == apiParameter.Name);
                if (parameter != null)
                {
                    parameter.required = false;
                }
            }
        }
    }
}
