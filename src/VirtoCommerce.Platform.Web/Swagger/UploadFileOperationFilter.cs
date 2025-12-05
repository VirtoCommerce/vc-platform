using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Swagger;

namespace VirtoCommerce.Platform.Web.Swagger;

/// <summary>
/// Swashbuckle operation filter that describes file upload endpoints
/// marked with <see cref="UploadFileAttribute"/> as multipart/form-data
/// with a binary file field.
/// </summary>
public class UploadFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var uploadAttribute = context.MethodInfo
            .GetCustomAttributes(typeof(UploadFileAttribute), inherit: true)
            .Cast<UploadFileAttribute>()
            .FirstOrDefault();

        if (uploadAttribute == null)
        {
            return;
        }

        operation.RequestBody ??= new OpenApiRequestBody();
        operation.RequestBody.Content ??= new Dictionary<string, OpenApiMediaType>();

        if (!operation.RequestBody.Content.TryGetValue("multipart/form-data", out var mediaType))
        {
            mediaType = new OpenApiMediaType();
            operation.RequestBody.Content["multipart/form-data"] = mediaType;
        }

        mediaType.Schema ??= new OpenApiSchema { Type = "object" };
        var schema = mediaType.Schema;

        var filePropertyName = string.IsNullOrEmpty(uploadAttribute.Name) ? "file" : uploadAttribute.Name;

        schema.Properties ??= new Dictionary<string, OpenApiSchema>();
        var fileSchema = CreateFileSchema(uploadAttribute);
        schema.Properties[filePropertyName] = fileSchema;

        if (uploadAttribute.Required)
        {
            schema.Required ??= new HashSet<string>();
            if (!schema.Required.Contains(filePropertyName))
            {
                schema.Required.Add(filePropertyName);
            }

            operation.RequestBody.Required = true;
        }
    }

    private static OpenApiSchema CreateFileSchema(UploadFileAttribute uploadAttribute)
    {
        var elementType = string.IsNullOrEmpty(uploadAttribute.Type) ? "string" : uploadAttribute.Type;

        if (uploadAttribute.AllowMultiple)
        {
            // array of binary strings
            return new OpenApiSchema
            {
                Type = "array",
                Description = uploadAttribute.Description,
                Items = new OpenApiSchema
                {
                    Type = elementType,
                    Format = "binary",
                },
            };
        }

        // single binary string
        return new OpenApiSchema
        {
            Type = elementType,
            Format = "binary",
            Description = uploadAttribute.Description,
        };
    }
}


