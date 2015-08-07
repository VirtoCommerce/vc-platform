using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Swashbuckle.Application;
using System.Net.Http;

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
			var xmlPath = System.String.Format(@"{0}\bin\VirtoCommerce.Platform.Web.XML", System.AppDomain.CurrentDomain.BaseDirectory);
			GlobalConfiguration.Configuration.
				 EnableSwagger(
				 c =>
				 {
					
					 c.IgnoreObsoleteProperties();
					 c.UseFullTypeNameInSchemaIds();
					 c.DescribeAllEnumsAsStrings();
					 c.SingleApiVersion("v1", "VirtoCommerce Platform Web documentation");
					 c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
					 c.RootUrl(req => new Uri(req.RequestUri, req.GetRequestContext().VirtualPathRoot).ToString());
					 c.ApiKey("apiKey")
						 .Description("API Key Authentication")
						 .Name("api_key")
						 .In("header");
				 }
				 ).EnableSwaggerUi();

        }

        #endregion

    }
}
