using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Swagger;

/// <summary>
/// This operation filter assigns module info (such as name and id) to operations based on their module association.
/// </summary>
public class ModuleInfoFilter : IOperationFilter, IDocumentFilter
{
    private const string _moduleIdExtension = "x-virtocommerce-module-id";

    private readonly Dictionary<string, SwaggerModule> _moduleByTitle;
    private readonly Dictionary<string, SwaggerModule> _moduleByAssemblyName;

    /// <summary>
    /// This operation filter assigns module info (such as name and id) to operations based on their module association.
    /// </summary>
    public ModuleInfoFilter(IModuleCatalog moduleCatalog)
    {
        var modules = moduleCatalog.Modules
            .OfType<ManifestModuleInfo>()
            .Where(x => x.ModuleInstance != null)
            .Select(x =>
                new SwaggerModule
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    AssemblyName = x.ModuleInstance.GetType().Assembly.GetName().Name,
                })
            .ToList();

        modules.Insert(0, new SwaggerModule
        {
            Id = "VirtoCommerce.Platform",
            Title = "VirtoCommerce Platform",
            AssemblyName = GetType().Assembly.GetName().Name,
        });

        _moduleByTitle = modules.ToDictionary(x => x.Title);
        _moduleByAssemblyName = modules.ToDictionary(x => x.AssemblyName);
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerAssemblyName = ((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerTypeInfo.Assembly.GetName().Name;

        if (_moduleByAssemblyName.TryGetValueSafe(controllerAssemblyName, out var module))
        {
            operation.Tags = [new OpenApiTag { Name = module.Title }];
            operation.Extensions[_moduleIdExtension] = new OpenApiString(module.Id);
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

        // Add tags at document level with descriptions and extensions
        foreach (var tagName in operationTagNames)
        {
            if (_moduleByTitle.TryGetValue(tagName, out var module))
            {
                swaggerDoc.Tags.Add(new OpenApiTag
                {
                    Name = module.Title,
                    Description = module.Description,
                    Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        { _moduleIdExtension, new OpenApiString(module.Id) },
                    },
                });
            }
        }
    }

    private sealed class SwaggerModule
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string AssemblyName { get; init; }
    }
}
