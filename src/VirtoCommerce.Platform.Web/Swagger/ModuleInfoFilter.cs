using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// This operation filter assigns module info (such as name and id) to operations based on their module association.
    /// </summary>
    public class ModuleInfoFilter : IOperationFilter, IDocumentFilter
    {
        private const string _moduleIdExtension = "x-virtocommerce-module-id";
        private const string _platformTag = "VirtoCommerce Platform";
        private const string _platformId = "VirtoCommerce.Platform";

        private readonly IModuleCatalog _moduleCatalog;

        public ModuleInfoFilter(IModuleCatalog moduleCatalog)
        {
            _moduleCatalog = moduleCatalog;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerTypeInfo = ((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerTypeInfo;
            var module = _moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => x.ModuleInstance != null)
                .FirstOrDefault(x => (controllerTypeInfo.Assembly == x.ModuleInstance.GetType().Assembly));

            if (module != null)
            {
                operation.Tags = new List<OpenApiTag>
                {
                    new OpenApiTag() { Name = module.Title } // Tags is an array of strings at operation level
                };

                operation.Extensions[_moduleIdExtension] = new OpenApiString(module.Id);
            }
            else if (controllerTypeInfo.Assembly.GetName().Name == typeof(ModuleInfoFilter).Assembly.GetName().Name)
            {
                operation.Tags = new List<OpenApiTag>
                {
                    new OpenApiTag() { Name = _platformTag }
                };

                operation.Extensions[_moduleIdExtension] = new OpenApiString(_platformId);
            }
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags ??= [];

            // Collect unique tag names from operations
            var operationTagNames = swaggerDoc.Paths
                .SelectMany(path => path.Value.Operations.Values)
                .SelectMany(operation => operation.Tags ?? [])
                .Select(tag => tag.Name)
                .Distinct()
                .ToList();

            // Build a mapping of tag names to module info
            var tagToModuleMap = new Dictionary<string, ManifestModuleInfo>();

            var modules = _moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => x.ModuleInstance != null)
                .ToList();

            foreach (var module in modules)
            {
                tagToModuleMap[module.Title] = module;
            }

            // Add tags to document level with descriptions and extensions
            foreach (var tagName in operationTagNames)
            {
                if (tagToModuleMap.TryGetValue(tagName, out var module))
                {
                    swaggerDoc.Tags.Add(new OpenApiTag
                    {
                        Name = module.Title,
                        Description = module.Description,
                        Extensions = new Dictionary<string, IOpenApiExtension>
                        {
                            { _moduleIdExtension, new OpenApiString(module.Id) }
                        }
                    });
                }
                else if (tagName == _platformTag)
                {
                    swaggerDoc.Tags.Add(new OpenApiTag
                    {
                        Name = _platformTag,
                        Extensions = new Dictionary<string, IOpenApiExtension>
                        {
                            { _moduleIdExtension, new OpenApiString(_platformTag) }
                        }
                    });
                }
            }
        }
    }
}
