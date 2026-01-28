using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Web.Controllers.Api;

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
                // Create Properties dictionary with IOpenApiSchema interface type to avoid InvalidCastException
                var schemaProperties = new Dictionary<string, IOpenApiSchema>
                {
                    { "grant_type", new OpenApiSchema() { Type = JsonSchemaType.String } },
                    { "scope", new OpenApiSchema() { Type = JsonSchemaType.String } },
                    { "username", new OpenApiSchema() { Type = JsonSchemaType.String } },
                    { "password", new OpenApiSchema() { Type = JsonSchemaType.String } },
                    { "storeId", new OpenApiSchema() { Type = JsonSchemaType.String } },
                    { "user_id", new OpenApiSchema() { Type = JsonSchemaType.String } },
                };

                var mediaType = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Required = new HashSet<string>(["grant_type"]),
                        Type = JsonSchemaType.Object,
                        Properties = schemaProperties
                    }
                };

                // Create Content dictionary - in Microsoft.OpenAPI 2.0, Content expects Dictionary<string, OpenApiMediaType>
                // In 3.0+, it expects Dictionary<string, IOpenApiMediaType>
                var content = new Dictionary<string, OpenApiMediaType>
                {
                    { "application/x-www-form-urlencoded", mediaType }
                };

                // Create RequestBody with Content property set during initialization
                // since Content is read-only in OpenAPI v3
                operation.RequestBody = new OpenApiRequestBody
                {
                    Required = true,
                    Content = content
                };
            }
        }
    }
}
