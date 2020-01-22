using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;

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
                }
            }
        }
    }
}
