using System.IO;
using System.Linq;
using System.Net;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class FileResponseTypeFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (IsFileResponse(context))
            {
                var key = ((int)HttpStatusCode.OK).ToString();
                var responseSchema = new Schema { Format = "byte", Type = "file" };

                if (operation.Responses.TryGetValue(key, out var response))
                {
                    response.Schema = responseSchema;
                    return;
                }

                operation.Responses.Add(key, new Response
                {
                    Description = "OK",
                    Schema = responseSchema
                });
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
