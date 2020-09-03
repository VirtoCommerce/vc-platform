using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
