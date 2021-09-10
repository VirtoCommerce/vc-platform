using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Mvc.Server;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// This filter adds parameters description for "~/connect/token" endpoint
    /// </summary>
    public class OpenIDEndpointDescriptionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && nameof(AuthorizationController).StartsWith(descriptor.ControllerName) && descriptor.ActionName.Equals(nameof(AuthorizationController.Exchange)))
            {
                // In OpenAPI 3.0, form data passed thru an object schema where the object properties represent the form fields.
                // The code below implemented as prescribed at the page https://swagger.io/docs/specification/describing-request-body/
                operation.RequestBody = new OpenApiRequestBody();
                operation.RequestBody.Required = true;
                operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>();
                var mediaType = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema()
                };
                mediaType.Schema.Type = "object";
                mediaType.Schema.Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "grant_type", new OpenApiSchema() { Type = "string" } },
                    { "scope", new OpenApiSchema() { Type = "string" } },
                    { "username", new OpenApiSchema() { Type = "string" } },
                    { "password", new OpenApiSchema() { Type = "string" } }
                };
                mediaType.Schema.Required = new HashSet<string>
                {
                    "grant_type"
                };
                operation.RequestBody.Content.Add("application/x-www-form-urlencoded", mediaType);
            }
        }
    }
}
