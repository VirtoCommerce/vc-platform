using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;
using CacheManager.Core;
using Microsoft.Practices.Unity;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public static class SwaggerConfig
    {
        public static void RegisterRoutes(IUnityContainer container)
        {
            var httpConfiguration = container.Resolve<HttpConfiguration>();
            var moduleInitializerOptions = container.Resolve<IModuleInitializerOptions>();
            var routePrefix = moduleInitializerOptions.RoutePrefix;

            var xmlCommentsDirectoryPaths = new[]
            {
                moduleInitializerOptions.VirtualRoot + "/App_Data/Modules",
                moduleInitializerOptions.VirtualRoot + "/bin"
            };
            var xmlCommentsFilePaths = xmlCommentsDirectoryPaths.SelectMany(GetXmlFilesPaths).ToArray();

            // Add separate swagger generator for platform
            EnableSwagger("VirtoCommerce.Platform", httpConfiguration, container, routePrefix, xmlCommentsFilePaths, false, Assembly.GetExecutingAssembly());

            // Add separate swagger generator for each installed module
            foreach (var module in container.Resolve<IModuleCatalog>().Modules.OfType<ManifestModuleInfo>().Where(m => m.ModuleInstance != null))
            {
                EnableSwagger(module.ModuleName, httpConfiguration, container, routePrefix, xmlCommentsFilePaths, module.UseFullTypeNameInSwagger, module.ModuleInstance.GetType().Assembly);
            }
           
            // Add full swagger generator
            httpConfiguration.EnableSwagger(routePrefix + "docs/{apiVersion}", c =>
            {
                Func<TagsFilter> tagsFilterFactory = () => new TagsFilter(container.Resolve<IModuleCatalog>(), container.Resolve<ISettingsManager>());

                c.SingleApiVersion("v1", "VirtoCommerce Solution REST API documentation");
                c.UseFullTypeNameInSchemaIds();
                c.DocumentFilter(tagsFilterFactory);
                c.OperationFilter(tagsFilterFactory);
                ApplyCommonSwaggerConfiguration(c, container, string.Empty, xmlCommentsFilePaths);
            })
            .EnableSwaggerUi(routePrefix + "docs/ui/{*assetPath}", c =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                const string resourcePrefix = "VirtoCommerce.Platform.Web.Swagger.UI.";

                c.CustomAsset("index", assembly, resourcePrefix + "index.html");
                c.CustomAsset("images/logo_small-png", assembly, resourcePrefix + "logo_small.png");
                c.CustomAsset("css/vc-css", assembly, resourcePrefix + "vc.css");
                c.CustomAsset("swagger-ui-min-js", assembly, resourcePrefix + "swagger-ui.min.js");
            });
        }

        private static void EnableSwagger(string moduleName, HttpConfiguration httpConfiguration, IUnityContainer container, string routePrefix, string[] xmlCommentsFilePaths, bool useFullTypeNameInSchemaIds, Assembly apiAssembly)
        {
            var routeName = string.Concat("swagger_", moduleName);
            var routeTemplate = string.Concat(routePrefix, "docs/", moduleName, "/{apiVersion}");

            httpConfiguration.EnableSwagger(routeName, routeTemplate, c =>
            {
                // Include only APIs from given assembly
                c.MultipleApiVersions(
                    (apiDescription, apiVersion) => apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly == apiAssembly,
                    versionInfoBuilder => versionInfoBuilder.Version("v1", moduleName + " REST API documentation"));

                if (useFullTypeNameInSchemaIds)
                {
                    c.UseFullTypeNameInSchemaIds();
                }

                ApplyCommonSwaggerConfiguration(c, container, moduleName, xmlCommentsFilePaths);
                c.OperationFilter(() => new ModuleTagsFilter(moduleName));
            });
        }

        private static void ApplyCommonSwaggerConfiguration(SwaggerDocsConfig c, IUnityContainer container, string cacheKey, string[] xmlCommentsFilePaths)
        {
            var cacheManager = container.Resolve<ICacheManager<object>>();

            c.CustomProvider(defaultProvider => new CachingSwaggerProvider(defaultProvider, cacheManager, cacheKey));
            c.MapType<object>(() => new Schema { type = "object" });
            c.IgnoreObsoleteProperties();
            c.DescribeAllEnumsAsStrings();
            c.OperationFilter(() => new OptionalParametersFilter());
            c.OperationFilter(() => new FileResponseTypeFilter());
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.RootUrl(message => new Uri(message.RequestUri, message.GetRequestContext().VirtualPathRoot).ToString());
            c.PrettyPrint();
            c.ApiKey("apiKey")
                .Description("API Key Authentication")
                .Name("api_key")
                .In("header");

            foreach (var path in xmlCommentsFilePaths)
            {
                c.IncludeXmlComments(path);
            }
        }

        private static string[] GetXmlFilesPaths(string virtualDirectoryPath)
        {
            var physicalPath = HostingEnvironment.MapPath(virtualDirectoryPath);
            return physicalPath != null ? Directory.GetFiles(physicalPath, "*.Web.XML") : new string[] { };
        }
    }
}
