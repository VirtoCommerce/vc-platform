using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// This temporary filter removes broken "application/*+json" content-type.
    /// It seems it's some openapi/swagger bug, because Autorest fails.        
    /// </summary>
    public class ConsumeFromBodyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody != null && operation.RequestBody.Content.Count > 0 && operation.RequestBody.Content.ContainsKey("application/*+json"))
            {
                operation.RequestBody.Content.Remove("application/*+json");
            }
        }
    }
}
