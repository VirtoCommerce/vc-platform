using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly string _modulesVirtualPath;
        private readonly string _modulesPhysicalPath;
        private readonly string _assembliesPath;

        public VirtoCommercePlatformWebBootstraper(string modulesVirtualPath, string modulesPhysicalPath, string assembliesPath)
        {
            _modulesVirtualPath = modulesVirtualPath;
            _modulesPhysicalPath = modulesPhysicalPath;
            _assembliesPath = assembliesPath;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var manifestProvider = new ModuleManifestProvider(_modulesPhysicalPath);
            return new ManifestModuleCatalog(manifestProvider, _modulesVirtualPath, _assembliesPath);
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            var options = new ModuleInitializerOptions();
            Container.RegisterInstance<IModuleInitializerOptions>(options);
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

            var moduleCatalog = ModuleCatalog as ManifestModuleCatalog;
            if (moduleCatalog != null)
            {
                Container.RegisterInstance(moduleCatalog.ManifestProvider);
            }

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
