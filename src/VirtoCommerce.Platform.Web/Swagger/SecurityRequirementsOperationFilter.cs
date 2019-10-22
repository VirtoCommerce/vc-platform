using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Where(a => !string.IsNullOrEmpty(a.Policy))
                .Select(attr => attr.Policy)
                .Distinct().ToArray();

            if (!requiredScopes.IsNullOrEmpty())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>();
                var oauth2SecurityRequirement = new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme() { Type = SecuritySchemeType.OAuth2 }, requiredScopes }
                };
                operation.Security.Add(oauth2SecurityRequirement);
            }
        }
    }
}
