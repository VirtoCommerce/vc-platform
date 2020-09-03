using System;
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
        private string _documentName;

        public CustomSwaggerGenerator(SwaggerGeneratorOptions options, IApiDescriptionGroupCollectionProvider apiDescriptionsProvider, ISchemaGenerator schemaGenerator)
        {
            // We change schema generator options to replace SchemaIdSelector function to a document-dependent implementation
            // Unfortunately this member declared as private, there is no another way to obtain options in this member.
            var optionsFieldInfo = schemaGenerator.GetType().GetField("_generatorOptions", BindingFlags.Instance | BindingFlags.NonPublic);
            var schemaGeneratorOptions = (SchemaGeneratorOptions)optionsFieldInfo.GetValue(schemaGenerator);

            var defaultSchemaIdSelectorMethodInfo = schemaGeneratorOptions.GetType().GetMethod("DefaultSchemaIdSelector", BindingFlags.Instance | BindingFlags.NonPublic);
            schemaGeneratorOptions.SchemaIdSelector = (
                type =>
                (Attribute.GetCustomAttribute(type, typeof(SwaggerSchemaIdAttribute)) as SwaggerSchemaIdAttribute)?.Id ??
                (
                    _documentName == SwaggerServiceCollectionExtensions.platformUIDocName ?
                     type.FullName :
                    (string)defaultSchemaIdSelectorMethodInfo.Invoke(schemaGeneratorOptions, new object[] { type })
                )
                );

            _swaggerGenerator = new SwaggerGenerator(options, apiDescriptionsProvider, schemaGenerator);
        }

        public OpenApiDocument GetSwagger(string documentName, string host = null, string basePath = null)
        {
            // Just catch document name
            _documentName = documentName;
            return _swaggerGenerator.GetSwagger(documentName, host, basePath);
        }
    }
}
