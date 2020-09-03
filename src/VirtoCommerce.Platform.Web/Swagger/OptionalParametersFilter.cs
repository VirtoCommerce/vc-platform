using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
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
                .Where(p => p.ParameterDescriptor != null &&
                ((ControllerParameterDescriptor)p.ParameterDescriptor).ParameterInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(SwaggerOptionalAttribute))).ToList();

            foreach (var apiParameter in optionalParameters)
            {
                var parameter = operation.Parameters.FirstOrDefault(p => p.Name == apiParameter.Name);
                if (parameter != null)
                {
                    parameter.Required = false;
                }
            }
        }
    }
}
