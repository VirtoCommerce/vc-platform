using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Swagger;

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
                        Schema = new OpenApiSchema() { Type = attr.Type }
                    });
                }

                if (requestAttributes.Any(x => x.Type == "file"))
                {
                    // https://swagger.io/docs/specification/describing-request-body/file-upload/
                    operation.RequestBody = new OpenApiRequestBody()
                    {
                        Content = { ["multipart/form-data"] =
                            new OpenApiMediaType() {
                                    Schema = new OpenApiSchema() { Type = "object",
                                    Properties = { ["file"] = new OpenApiSchema() {
                                            Description = "Select file", Type = "string", Format = "binary"
                                        }
                                    }
                                }
                            }
                        }
                    };
                }
            }
        }
    }
}
