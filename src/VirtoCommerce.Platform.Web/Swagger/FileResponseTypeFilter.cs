using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class FileResponseTypeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (IsFileResponse(context))
            {
                var key = ((int)HttpStatusCode.OK).ToString();
                // Accordingly to: https://swagger.io/docs/specification/describing-responses/#response-that-returns-a-file
                // the type of response should have Format = "binary", Type = "string".
                var responseSchema = new OpenApiSchema { Format = "binary", Type = "string" };

                if (operation.Responses == null)
                {
                    operation.Responses = new OpenApiResponses();
                }

                if (!operation.Responses.TryGetValue(key, out var response))
                {
                    response = new OpenApiResponse();
                }

                response.Description = "OK";
                response.Content = new Dictionary<string, OpenApiMediaType>
                {

                    // TODO: (AK) ? Consider to correct content key depending on real MIME
                    { "application/octet-stream", new OpenApiMediaType() { Schema = responseSchema } }
                };
            }
        }

        private static bool IsFileResponse(OperationFilterContext context)
        {
            var fileResponseAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<SwaggerFileResponseAttribute>();

            var result = fileResponseAttributes.Any();

            if (!result)
            {
                result = context.ApiDescription.SupportedResponseTypes.Any(r => r.Type == typeof(Stream));
            }
            return result;
        }

    }
}
