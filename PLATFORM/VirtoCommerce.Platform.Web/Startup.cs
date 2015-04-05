using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Framework.Core.Utils;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Platform.Web;
using VirtoCommerce.Scheduling.Windows;
using WebGrease.Extensions;

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string modulesVirtualPath = "~/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath).EnsureEndSeparator();
            var assembliesPath = HostingEnvironment.MapPath("~/App_data/Modules");

            var bootstraper = new VirtoCommercePlatformWebBootstraper(modulesVirtualPath, modulesPhysicalPath, assembliesPath);
            bootstraper.Run();
            bootstraper.Container.RegisterInstance(app);

            var moduleCatalog = bootstraper.Container.Resolve<IModuleCatalog>();

            // Register URL rewriter before modules initialization
            if (Directory.Exists(modulesPhysicalPath))
            {
                var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.EnsureEndSeparator();
                var modulesRelativePath = MakeRelativePath(applicationBase, modulesPhysicalPath);

                var urlRewriterOptions = new UrlRewriterOptions();
                var moduleInitializerOptions = (ModuleInitializerOptions)bootstraper.Container.Resolve<IModuleInitializerOptions>();
                moduleInitializerOptions.SampleDataLevel = EnumUtility.SafeParse(ConfigurationManager.AppSettings["VirtoCommerce:SampleDataLevel"], SampleDataLevel.None);

                foreach (var module in moduleCatalog.Modules.OfType<ManifestModuleInfo>())
                {
                    var urlRewriteKey = string.Format(CultureInfo.InvariantCulture, "/Modules/$({0})", module.ModuleName);
                    var urlRewriteValue = MakeRelativePath(modulesPhysicalPath, module.FullPhysicalPath);
                    urlRewriterOptions.Items.Add(PathString.FromUriComponent(urlRewriteKey), "/" + urlRewriteValue);

                    moduleInitializerOptions.ModuleDirectories.Add(module.ModuleName, module.FullPhysicalPath);
                }

                app.Use<UrlRewriterOwinMiddleware>(urlRewriterOptions);
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileSystem = new PhysicalFileSystem(modulesRelativePath)
                });
            }

            // Ensure all modules are loaded
            var moduleManager = bootstraper.Container.Resolve<IModuleManager>();

            foreach (var module in moduleCatalog.Modules.Where(x => x.State == ModuleState.NotStarted))
            {
                moduleManager.LoadModule(module.ModuleName);
            }

            SchedulerHost.CreateScheduler(bootstraper.Container, ConfigurationManager.AppSettings["SchedulerHost"]);
        }


        private static string MakeRelativePath(string rootPath, string fullPath)
        {
            var rootUri = new Uri(rootPath);
            var fullUri = new Uri(fullPath);
            var relativePath = rootUri.MakeRelativeUri(fullUri).ToString();
            return relativePath;
        }
    }
}
