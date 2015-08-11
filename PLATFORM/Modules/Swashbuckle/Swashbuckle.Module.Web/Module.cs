using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Swashbuckle.Application;
using System.Net.Http;
using VirtoCommerce.Platform.Core.Packaging;
using Swashbuckle.Swagger;
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
			var xmlRelativePaths = new[] { "~/App_Data/Modules", "~/bin" };
			Func<PopulateTagsFilter> tagsFilterFactory = () => new PopulateTagsFilter(_container.Resolve<IPackageService>(), _container.Resolve<ISettingsManager>());
			GlobalConfiguration.Configuration.
				 EnableSwagger(
				 c =>
				 {
					 foreach (var xmlRelativePath in xmlRelativePaths)
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
					 c.SingleApiVersion("v1", "VirtoCommerce Platform RESTful API documentation");
					 c.DocumentFilter(tagsFilterFactory);
					 c.OperationFilter(tagsFilterFactory);
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

		private string[] GetXmlFilesPaths(string xmlRelativePath)
		{
			var path = HostingEnvironment.MapPath(xmlRelativePath);
			var files = Directory.GetFiles(path, "*.Web.XML");
			return files;
		}
		private string GroupAction(System.Web.Http.Description.ApiDescription apiDescriptor)
		{
			return apiDescriptor.ActionDescriptor.ControllerDescriptor.ControllerName;
		}

		private class PopulateTagsFilter : IDocumentFilter, IOperationFilter
		{
			private readonly IPackageService _packageService;
			private readonly ISettingsManager _settingManager;
			public PopulateTagsFilter(IPackageService packageService, ISettingsManager settingManager)
			{
				_packageService = packageService;
				_settingManager = settingManager;
			}
			#region IDocumentFilter Members

			public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
			{
				var defaultApiKey = _settingManager.GetValue("Swashbuckle.DefaultApiKey", string.Empty);

				swaggerDoc.info.description = string.Format("For this sample, you can use the `{0}` key to test the authorization filters.", defaultApiKey);
				swaggerDoc.info.contact = new Contact
				{
					email = "support@virtocommerce.com",
					name = "VirtoCommerce",
					url = "http://virtocommerce.com"
				};
				swaggerDoc.info.termsOfService = "";
				swaggerDoc.info.license = new License
				{
					name = "Virto Commerce Open Software License 3.0",
					url = "http://virtocommerce.com/opensourcelicense"
				};
				var tags = _packageService.GetModules().Select(x => new Tag
					{
						name = x.Title,
						description = x.Description
					}).ToList();
				tags.Add(new Tag
				{
					name = "VirtoCommerce platform",
					description = "Platform functionality represent common resources and operations"
				});
				swaggerDoc.tags = tags;

			}

			#endregion

			#region IOperationFilter Members

			public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
			{
				var module = _packageService.GetModules().Where(x => x.ModuleInfo.ModuleInstance != null).FirstOrDefault(x => apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly == x.ModuleInfo.ModuleInstance.GetType().Assembly);
				if (module != null)
				{
					operation.tags = new string[] { module.Title };
				}
				else if (apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly.GetName().Name == "VirtoCommerce.Platform.Web")
				{
					operation.tags = new string[] { "VirtoCommerce platform" };
				}
			}

			#endregion
		}

	}


}
