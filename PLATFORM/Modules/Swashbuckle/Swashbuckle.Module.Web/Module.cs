using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Swashbuckle.Application;
using System.Net.Http;
using System.Reflection;
using System.Web.Hosting;
using System.IO;

namespace SwashbuckleModule.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            var xmlRelativePath = settingsManager.GetValue("Swashbuckle.XmlRelativePath", string.Empty);
            var defaultApiKey = settingsManager.GetValue("Swashbuckle.DefaultApiKey", string.Empty);
            GlobalConfiguration.Configuration.
				 EnableSwagger(
				 c =>
				 {
                     if (!string.IsNullOrEmpty(xmlRelativePath))
                     {
                         var xmlFilesPaths = GetXmlFilesPaths(xmlRelativePath);
                         foreach (var path in xmlFilesPaths)
                         {
                             c.IncludeXmlComments(path);
                         }
                     }
					 c.IgnoreObsoleteProperties();
					 c.UseFullTypeNameInSchemaIds();
					 c.DescribeAllEnumsAsStrings();
                     c.SingleApiVersion("v1", string.Format("VirtoCommerce Platform Web documentation. For this sample, you can use the {0} special-key to test the authorization filters.", defaultApiKey));
					 c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
					 c.RootUrl(req => new Uri(req.RequestUri, req.GetRequestContext().VirtualPathRoot).ToString());
					 c.ApiKey("apiKey")
						 .Description("API Key Authentication")
						 .Name("api_key")
						 .In("header");
				 }
                 ).EnableSwaggerUi(c =>
                 {
                 }
                 );

        }

        private string[] GetXmlFilesPaths(string xmlRelativePath)
        {
            var path = HostingEnvironment.MapPath(xmlRelativePath);
            var files = Directory.GetFiles(path, "*.Web.XML");
            return files;
        }

        #endregion

    }
}
