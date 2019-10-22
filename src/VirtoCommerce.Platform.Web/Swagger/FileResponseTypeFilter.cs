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
    public class FileResponseTypeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // https://swagger.io/docs/specification/describing-responses/
            if (IsFileResponse(context))
            {
                var key = ((int)HttpStatusCode.OK).ToString();
                var responseSchema = new OpenApiSchema { Format = "binary", Type = "file" };
                
                if (operation.Responses.TryGetValue(key, out var response))
                {
                    response.Content.FirstOrDefault().Value.Schema = responseSchema;
                    return;
                }

                var openApiResponse = new OpenApiResponse
                {
                    Description = "OK",
                    Content = new Dictionary<string, OpenApiMediaType>()
                };
                // TODO: (AK) ? Consider to correct content key depending on real MIME
                openApiResponse.Content.Add("multipart/form-data", new OpenApiMediaType() { Schema = responseSchema });
                operation.Responses.Add(key, openApiResponse);
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
