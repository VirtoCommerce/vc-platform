using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Swagger;

namespace VirtoCommerce.Platform.Web.Swagger;

/// <summary>
/// Swashbuckle operation filter that describes file upload endpoints
/// marked with <see cref="UploadFileAttribute"/> as multipart/form-data
/// with a binary file field.
/// <para>
/// This implementation is compatible with Microsoft.OpenApi 2.x+,
/// where schema <c>Type</c> is based on <see cref="JsonSchemaType"/>
/// and request body <c>Content</c> is modeled via OpenAPI 3.0.
/// </para>
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

        var filePropertyName = string.IsNullOrEmpty(uploadAttribute.Name) ? "file" : uploadAttribute.Name;

        // Build the file schema for streamed multipart upload
        var fileSchema = CreateFileSchema(uploadAttribute);

        // Create Properties dictionary with IOpenApiSchema interface type to stay
        // compatible across Microsoft.OpenApi 2.x+ where the underlying schema
        // collection types have changed.
        var schemaProperties = new Dictionary<string, IOpenApiSchema>
        {
            [filePropertyName] = fileSchema,
        };

        HashSet<string> required = null;
        if (uploadAttribute.Required)
        {
            required = new HashSet<string> { filePropertyName };
        }

        var requestSchema = new OpenApiSchema
        {
            Type = JsonSchemaType.Object,
            Properties = schemaProperties,
            Required = required,
        };

        var mediaType = new OpenApiMediaType
        {
            Schema = requestSchema,
        };

        // In Microsoft.OpenApi 2.x+, RequestBody.Content uses OpenAPI 3.0 model and
        // is safest to set during initialization to support newer read-only patterns.
        var content = new Dictionary<string, OpenApiMediaType>
        {
            ["multipart/form-data"] = mediaType,
        };

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = uploadAttribute.Required,
            Content = content,
        };
    }

    private static OpenApiSchema CreateFileSchema(UploadFileAttribute uploadAttribute)
    {
        var elementType = ResolveElementJsonSchemaType(uploadAttribute.Type);

        if (uploadAttribute.AllowMultiple)
        {
            // array of binary strings
            return new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
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

    private static JsonSchemaType ResolveElementJsonSchemaType(string type)
    {
        if (string.IsNullOrEmpty(type))
        {
            return JsonSchemaType.String;
        }

        // For streaming uploads, OpenAPI recommends "string" with "binary" format.
        // We only support a minimal mapping here; everything else falls back to string.
        return type.ToLowerInvariant() switch
        {
            "string" => JsonSchemaType.String,
            "integer" => JsonSchemaType.Integer,
            "number" => JsonSchemaType.Number,
            "boolean" => JsonSchemaType.Boolean,
            "object" => JsonSchemaType.Object,
            "array" => JsonSchemaType.Array,
            _ => JsonSchemaType.String,
        };
    }
}
