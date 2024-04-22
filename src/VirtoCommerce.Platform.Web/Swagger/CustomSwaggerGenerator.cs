using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Swagger;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// Generate swagger schema ids for schema refs depending on document name
    /// </summary>
    public class CustomSwaggerGenerator : ISwaggerProvider
    {
        private readonly SwaggerGenerator _swaggerGenerator;
        private readonly SchemaGeneratorOptions _schemaGeneratorOptions;
        private readonly MethodInfo _defaultSchemaIdSelectorMethodInfo;

        public CustomSwaggerGenerator(SwaggerGeneratorOptions options, IApiDescriptionGroupCollectionProvider apiDescriptionsProvider, ISchemaGenerator schemaGenerator)
        {
            // We change schema generator options to replace SchemaIdSelector function to a document-dependent implementation
            // Unfortunately this member declared as private, there is no another way to obtain options in this member.
            var optionsFieldInfo = schemaGenerator.GetType().GetField("_generatorOptions", BindingFlags.Instance | BindingFlags.NonPublic);
            _schemaGeneratorOptions = (SchemaGeneratorOptions)optionsFieldInfo.GetValue(schemaGenerator);

            _defaultSchemaIdSelectorMethodInfo = _schemaGeneratorOptions.GetType().GetMethod("DefaultSchemaIdSelector", BindingFlags.Instance | BindingFlags.NonPublic);

            _swaggerGenerator = new SwaggerGenerator(options, apiDescriptionsProvider, schemaGenerator);
        }

        public OpenApiDocument GetSwagger(string documentName, string host = null, string basePath = null)
        {
            SetSchemaIdSelector(documentName);
            return _swaggerGenerator.GetSwagger(documentName, host, basePath);
        }

        private void SetSchemaIdSelector(string documentName)
        {
            _schemaGeneratorOptions.SchemaIdSelector = type =>
                {
                    string result;
                    var attribute = (SwaggerSchemaIdAttribute)Attribute.GetCustomAttribute(type, typeof(SwaggerSchemaIdAttribute));
                    if (attribute != null)
                    {
                        result = attribute.Id;
                    }
                    else if (documentName == SwaggerServiceCollectionExtensions.platformUIDocName)
                    {
                        result = type.FullName;
                    }
                    else
                    {
                        result = (string)_defaultSchemaIdSelectorMethodInfo.Invoke(_schemaGeneratorOptions, new object[] { type });
                    }
                    Trace.WriteLine($"SchemaIdSelector: {type.FullName} => {result}");
                    return result;
                };
        }
    }
}
