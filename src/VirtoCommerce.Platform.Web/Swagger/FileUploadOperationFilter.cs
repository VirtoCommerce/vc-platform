using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    //[CLSCompliant(false)]
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.TryGetMethodInfo(out var methodInfo))
            {
                var requestAttributes = methodInfo.GetCustomAttributes<UploadFileAttribute>().ToArray();
                operation.Parameters ??= new List<OpenApiParameter>();

                foreach (var attr in requestAttributes)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = attr.Name,
                        Description = attr.Description,
                        In = ParameterLocation.Query,
                        Required = attr.Required,
                        Schema = new OpenApiSchema() {Type = attr.Type }
                    });
                }

                if (requestAttributes.Any(x => x.Type == "file"))
                {
                    // https://swagger.io/docs/specification/describing-request-body/file-upload/
                    operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>();
                    // TODO: (AK) ? Consider to correct content key depending on real MIME
                    operation.RequestBody.Content.Add("multipart/form-data", new OpenApiMediaType() { Schema = new OpenApiSchema { Format = "binary", Type = "file" } });
                }
            }
        }
    }
}
