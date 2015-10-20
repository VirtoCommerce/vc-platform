using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront;
using VirtoCommerce.Storefront.App_Start;
using VirtoCommerce.Storefront.Models;
using VirtoCommerce.Storefront.OwinMiddlewares;
using Microsoft.Practices.Unity;

[assembly: OwinStartup(typeof(Startup))]
[assembly: PreApplicationStartMethod(typeof(Startup), "PreApplicationStart")]

namespace VirtoCommerce.Storefront
{
    public partial class Startup
    {
        private static readonly List<string> _directories = new List<string>(new[] { Path.Combine(HostingEnvironment.MapPath("~/App_Data"), Environment.Is64BitProcess ? "x64" : "x86") });
        private static Assembly _managerAssembly;

        public static void PreApplicationStart()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolve;

            var managerAssemblyPath = HostingEnvironment.MapPath("~/Areas/Admin/bin/VirtoCommerce.Platform.Web.dll");
            if (managerAssemblyPath != null && File.Exists(managerAssemblyPath))
            {
                _directories.Add(HostingEnvironment.MapPath("~/Areas/Admin/bin"));

                _managerAssembly = Assembly.LoadFrom(managerAssemblyPath);
                BuildManager.AddReferencedAssembly(_managerAssembly);
            }
        }

        public void Configuration(IAppBuilder app)
        {
            if (_managerAssembly != null)
            {
                AreaRegistration.RegisterAllAreas();
                CallChildConfigure(app, _managerAssembly, "VirtoCommerce.Platform.Web.Startup", "Configuration", "~/areas/admin", "admin/");
            }

            //EnginesConfig.RegisterEngines(ViewEngines.Engines);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.ConfigureAuth(app);
            UnityWebActivator.Start();
            var container = UnityConfig.GetConfiguredContainer();
            //Owin middleware
            app.Use<ContextInitializationMiddleware>(container.Resolve<IWorkContext>(), container.Resolve<IStoreModuleApi>(), 
                                                    container.Resolve<IVirtoCommercePlatformApi>());
        }


        private static void CallChildConfigure(IAppBuilder app, Assembly assembly, string typeName, string methodName, string virtualRoot, string routPrefix)
        {
            var type = assembly.GetType(typeName);
            if (type != null)
            {
                var methodInfo = type.GetMethod(methodName, new[] { typeof(IAppBuilder), typeof(string), typeof(string) });
                if (methodInfo != null)
                {
                    var classInstance = Activator.CreateInstance(type, null);
                    var parameters = new object[] { app, virtualRoot, routPrefix };
                    var result = methodInfo.Invoke(classInstance, parameters);
                }
            }
        }

        private static Assembly Resolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            var assemblyName = new AssemblyName(args.Name);
            var fileName = assemblyName.Name + ".dll";

            foreach (var directoryPath in _directories)
            {
                var filePath = Path.Combine(directoryPath, fileName);
                if (File.Exists(filePath))
                {
                    assembly = Assembly.LoadFrom(filePath);
                    break;
                }
            }

            return assembly;
        }
    }
}
