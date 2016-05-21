using System;
using System.Globalization;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class TagsFilter : IDocumentFilter, IOperationFilter
    {
        private readonly IModuleCatalog _moduleCatalog;
        private readonly ISettingsManager _settingManager;

        public TagsFilter(IModuleCatalog moduleCatalog, ISettingsManager settingManager)
        {
            _moduleCatalog = moduleCatalog;
            _settingManager = settingManager;
        }

        #region IDocumentFilter Members

        [CLSCompliant(false)]
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            var defaultApiKey = _settingManager.GetValue("Swashbuckle.DefaultApiKey", string.Empty);

            swaggerDoc.info.description = string.Format(CultureInfo.InvariantCulture, "For this sample, you can use the `{0}` key to satisfy the authorization filters.", defaultApiKey);
            swaggerDoc.info.termsOfService = "";

            swaggerDoc.info.contact = new Contact
            {
                email = "support@virtocommerce.com",
                name = "Virto Commerce",
                url = "http://virtocommerce.com"
            };

            swaggerDoc.info.license = new License
            {
                name = "Virto Commerce Open Software License 3.0",
                url = "http://virtocommerce.com/opensourcelicense"
            };

            var tags = _moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Select(x => new Tag
                {
                    name = x.Title,
                    description = x.Description
                })
                .ToList();

            tags.Add(new Tag
            {
                name = "VirtoCommerce platform",
                description = "Platform functionality represent common resources and operations"
            });

            swaggerDoc.tags = tags;
        }

        #endregion

        #region IOperationFilter Members

        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var module = _moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => x.ModuleInstance != null)
                .FirstOrDefault(x => apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly == x.ModuleInstance.GetType().Assembly);

            if (module != null)
            {
                operation.tags = new[] { module.Title };
            }
            else if (apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly.GetName().Name == "VirtoCommerce.Platform.Web")
            {
                operation.tags = new[] { "VirtoCommerce platform" };
            }
        }

        #endregion
    }
}
