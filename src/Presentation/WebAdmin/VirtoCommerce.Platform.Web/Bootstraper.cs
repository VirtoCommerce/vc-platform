using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.WebApi;
using VirtoCommerce.Framework.Web;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.Platform.Web
{
	public class VirtoCommercePlatformWebBootstraper : UnityBootstrapper
	{
		protected override IModuleCatalog CreateModuleCatalog()
		{
			var assembliesPath = HostingEnvironment.MapPath("~/App_data/Modules");
			var contentVirtualPath = "~/Modules";
			var contentPhysicalPath = HostingEnvironment.MapPath(contentVirtualPath);

			return new ManifestModuleCatalog { AssembliesPath = assembliesPath, ContentVirtualPath = contentVirtualPath, ContentPhysicalPath = contentPhysicalPath };
		}

		public override void Run(bool runWithDefaultConfiguration)
		{
			base.Run(runWithDefaultConfiguration);

			//registering Unity for MVC
			//DependencyResolver.SetResolver(new UnityDependencyResolver(container));

			//registering Unity for web API
			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(Container);

			//It necessary because WEB API does not get assemblies from AppDomain.
			GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver(from m in ModuleCatalog.Modules select m));

			AreaRegistration.RegisterAllAreas();

			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);


			AuthConfig.RegisterAuth();
		}

		public class CustomAssemblyResolver : DefaultAssembliesResolver
		{
			private readonly IEnumerable<ModuleInfo> _modules;

			public CustomAssemblyResolver(IEnumerable<ModuleInfo> modules)
			{
				_modules = modules;
			}

			public override ICollection<Assembly> GetAssemblies()
			{
				var baseAssemblies = base.GetAssemblies();
				var assemblies = new List<Assembly>(baseAssemblies);
				assemblies.AddRange(_modules
					.Where(m => !string.IsNullOrEmpty(m.Ref))
					.Select(m => Assembly.LoadFrom(m.Ref))
				);

				return assemblies;
			}

		}
	}
}
