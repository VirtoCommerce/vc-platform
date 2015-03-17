using System;
using System.Globalization;
using System.Linq;
using System.Web.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Platform.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string modulesVirtualPath = "~/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath);

            var bootstraper = new VirtoCommercePlatformWebBootstraper(modulesVirtualPath, modulesPhysicalPath);
            bootstraper.Run();
            bootstraper.Container.RegisterInstance(app);

            var moduleCatalog = bootstraper.Container.Resolve<IModuleCatalog>();

            // Register URL rewriter before modules initialization
            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var modulesRelativePath = MakeRelativePath(applicationBase, modulesPhysicalPath);
            var urlRewriterOptions = new UrlRewriterOptions();

            foreach (var module in moduleCatalog.Modules.OfType<ManifestModuleInfo>())
            {
                var urlRewriteKey = string.Format(CultureInfo.InvariantCulture, "/Modules/$({0})", module.ModuleName);
                var urlRewriteValue = MakeRelativePath(applicationBase, module.FullPhysicalPath);
                urlRewriterOptions.Items.Add(PathString.FromUriComponent(urlRewriteKey), "/" + urlRewriteValue);
            }

            app.Use<UrlRewriterOwinMiddleware>(urlRewriterOptions);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem(modulesRelativePath)
            });

            // Ensure all modules are loaded
            var moduleManager = bootstraper.Container.Resolve<IModuleManager>();

            foreach (var module in moduleCatalog.Modules.Where(x => x.State == ModuleState.NotStarted))
            {
                moduleManager.LoadModule(module.ModuleName);
            }
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
