using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Swashbuckle.Application;
using System.Net.Http;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Packaging;
using System.Collections.Generic;

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

			Func<PopulateTagsFilter> tagsFilterFactory = () => new PopulateTagsFilter(_container.Resolve<IPackageService>());
			GlobalConfiguration.Configuration.
				EnableSwagger(
				c =>
				{
					c.IgnoreObsoleteProperties();
					c.UseFullTypeNameInSchemaIds();
					c.DescribeAllEnumsAsStrings();
					c.DocumentFilter(tagsFilterFactory);
					c.OperationFilter(tagsFilterFactory);
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

		private string GroupAction(System.Web.Http.Description.ApiDescription apiDescriptor)
		{
			return apiDescriptor.ActionDescriptor.ControllerDescriptor.ControllerName;
		}

		private class PopulateTagsFilter : IDocumentFilter, IOperationFilter
		{
			private readonly IPackageService _packageService;
			public PopulateTagsFilter(IPackageService packageService)
			{
				_packageService = packageService;
			}
			#region IDocumentFilter Members

			public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
			{
				swaggerDoc.tags = _packageService.GetModules().Select(x => new Tag
					{
						name = x.Id,
						description = x.Description
					}).ToArray();

			}

			#endregion

			#region IOperationFilter Members

			public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
			{
				var module = _packageService.GetModules().Where(x=>x.ModuleInfo.ModuleInstance != null).FirstOrDefault(x => apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly == x.ModuleInfo.ModuleInstance.GetType().Assembly);
				if (module != null)
				{
					operation.tags = new string[] { module.Id };
				}
			}

			#endregion
		}

	}


}
