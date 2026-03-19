using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Swagger;

/// <summary>
/// Simple wrapper to implement IOpenApiExtension for string values
/// </summary>
internal class OpenApiStringExtension : IOpenApiExtension
{
    private readonly string _value;

    public OpenApiStringExtension(string value)
    {
        _value = value;
    }

    public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
    {
        // Write the string value directly - OpenAPI writer supports string primitives
        writer.WriteValue(_value);
    }
}

internal sealed class SwaggerModule
{
    public string Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string AssemblyName { get; init; }
}

/// <summary>
/// This operation filter assigns module info (such as name and id) to operations based on their module association.
/// </summary>
public class ModuleInfoFilter : IOperationFilter, IDocumentFilter
{
    private const string _moduleIdExtension = "x-virtocommerce-module-id";

    private readonly Dictionary<string, SwaggerModule> _moduleById;
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
            Description = "B2B Innovation Platform",
            AssemblyName = GetType().Assembly.GetName().Name,
        });

        _moduleById = modules.ToDictionary(x => x.Id);
        _moduleByAssemblyName = modules.ToDictionary(x => x.AssemblyName);
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerAssemblyName = ((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerTypeInfo.Assembly.GetName().Name;

        if (_moduleByAssemblyName.TryGetValueSafe(controllerAssemblyName, out var module))
        {
            // Use HashSet for Tags since ISet<OpenApiTagReference> is not constructible with collection expression
            // OpenApiTagReference requires referenceId in constructor and Name is read-only
            operation.Tags = new HashSet<OpenApiTagReference> { new OpenApiTagReference(module.Title) };

            // Initialize Extensions if null (Extensions can be null in Microsoft.OpenAPI)
            operation.Extensions ??= new Dictionary<string, IOpenApiExtension>();
            // Use custom extension wrapper for string values in OpenAPI v3
            operation.Extensions[_moduleIdExtension] = new OpenApiStringExtension(module.Id);
        }
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach(var tag in swaggerDoc.Tags)
        {
            if (_moduleById.TryGetValue(tag.Name, out var module))
            {
                tag.Name = module.Title;
                tag.Description = module.Description;
            }
        }
    }
}
