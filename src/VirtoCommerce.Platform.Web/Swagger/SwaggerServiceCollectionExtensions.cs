using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Swagger;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public static class SwaggerServiceCollectionExtensions
    {
        private static string platformDocName = "VirtoCommerce.Platform";
        private static string oauth2SchemeName = "oauth2";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
            var modules = provider.GetService<IModuleCatalog>().Modules.OfType<ManifestModuleInfo>().Where(m => m.ModuleInstance != null).ToArray();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(platformDocName, new Info
                {
                    Title = "VirtoCommerce Solution REST API documentation",
                    Version = "v1",
                    TermsOfService = "",
                    Description = "For this sample, you can use the",
                    Contact = new Contact
                    {
                        Email = "support@virtocommerce.com",
                        Name = "Virto Commerce",
                        Url = "http://virtocommerce.com"
                    },
                    License = new License
                    {
                        Name = "Virto Commerce Open Software License 3.0",
                        Url = "http://virtocommerce.com/opensourcelicense"
                    }
                });

                foreach (var module in modules)
                {
                    c.SwaggerDoc(module.ModuleName, new Info { Title = $"{module.Id}", Version = "v1" });
                    c.OperationFilter<ModuleTagsFilter>(module.Id);
                }

                c.TagActionsBy(api => api.GroupByModuleName(services));
                c.DescribeAllEnumsAsStrings();
                c.IgnoreObsoleteProperties();
                c.IgnoreObsoleteActions();
                c.OperationFilter<FileResponseTypeFilter>();
                c.OperationFilter<OptionalParametersFilter>();
                c.OperationFilter<FileUploadOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<TagsFilter>();
                c.DocumentFilter<TagsFilter>();
                c.MapType<object>(() => new Schema { Type = "object" });
                c.AddModulesXmlComments(services);
                c.CustomSchemaIds(type => (Attribute.GetCustomAttribute(type, typeof(SwaggerSchemaIdAttribute)) as SwaggerSchemaIdAttribute)?.Id ?? type.FriendlyId());
                c.AddSecurityDefinition(oauth2SchemeName, new OAuth2Scheme
                {
                    Type = oauth2SchemeName,
                    Description = "OAuth2 Resource Owner Password Grant flow",
                    Flow = "password",
                    TokenUrl = $"{httpContextAccessor.HttpContext?.Request?.Scheme}://{httpContextAccessor.HttpContext?.Request?.Host}/connect/token",
                });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName.EqualsInvariant(platformDocName)) return true;

                    var currentAssembly = ((ControllerActionDescriptor)apiDesc.ActionDescriptor).ControllerTypeInfo.Assembly;
                    var module = modules.FirstOrDefault(m => m.ModuleName.EqualsInvariant(docName));
                    return module != null && module.Assembly == currentAssembly;
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public static void UseSwagger(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseSwagger(c =>
            {
                c.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            var modules = applicationBuilder.ApplicationServices.GetService<IModuleCatalog>().Modules.OfType<ManifestModuleInfo>().Where(m => m.ModuleInstance != null).ToArray();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{platformDocName}/swagger.json", platformDocName);
                foreach (var module in modules)
                {
                    c.SwaggerEndpoint($"{module.Id}/swagger.json", module.Id);
                }
                c.RoutePrefix = "docs";
                c.EnableValidator();
                c.IndexStream = () =>
                {
                    var type = typeof(Startup).GetTypeInfo().Assembly
                        .GetManifestResourceStream("VirtoCommerce.Platform.Web.wwwroot.swagger.index.html");
                    return type;
                };
                c.DocumentTitle = "VirtoCommerce Solution REST API documentation";
                c.InjectStylesheet("/swagger/vc.css");
                c.ShowExtensions();
                c.DocExpansion(DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);
                c.OAuthClientId(string.Empty);
                c.OAuthClientSecret(string.Empty);
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }


        /// <summary>
        /// grouping by Module Names in the ApiDescription
        /// with comparing Assemlies
        /// </summary>
        /// <param name="api"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IList<string> GroupByModuleName(this ApiDescription api, IServiceCollection services)
        {
            var providerSnapshot = services.BuildServiceProvider();
            var moduleCatalog = providerSnapshot.GetRequiredService<ILocalModuleCatalog>();

            // ------
            // Lifted from ApiDescriptionExtensions
            var actionDescriptor = api.GetProperty<ControllerActionDescriptor>();

            if (actionDescriptor == null)
            {
                actionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                api.SetProperty(actionDescriptor);
            }
            // ------

            var moduleAssembly = actionDescriptor?.ControllerTypeInfo.Assembly ?? Assembly.GetExecutingAssembly();
            var groupName = moduleCatalog.Modules.FirstOrDefault(m => m.ModuleInstance != null && m.Assembly == moduleAssembly);

            return new List<string> { groupName != null ? groupName.ModuleName : "Platform" };
        }

        /// <summary>
        /// Add Comments/Descriptions from XML-files in the ApiDescription
        /// </summary>
        /// <param name="options"></param>
        /// <param name="services"></param>
        private static void AddModulesXmlComments(this SwaggerGenOptions options, IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var localStorageModuleCatalogOptions = provider.GetService<IOptions<LocalStorageModuleCatalogOptions>>().Value;

            var xmlCommentsDirectoryPaths = new[]
            {
                localStorageModuleCatalogOptions.ProbingPath,
                AppContext.BaseDirectory
            };

            foreach (var path in xmlCommentsDirectoryPaths)
            {
                var xmlComments = Directory.GetFiles(path, "*.XML");
                foreach (var xmlComment in xmlComments)
                {
                    options.IncludeXmlComments(xmlComment);
                }
            }
        }
    }
}
