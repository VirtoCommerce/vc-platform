using System.Linq;
using Microsoft.Owin;
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
            var bootstraper = new VirtoCommercePlatformWebBootstraper();
            bootstraper.Run();

            bootstraper.Container.RegisterInstance(app);

            var moduleManager = bootstraper.Container.Resolve<IModuleManager>();
            var moduleCatalog = bootstraper.Container.Resolve<IModuleCatalog>();

            //Ensure all modules are loaded
            foreach (var module in moduleCatalog.Modules.Where(x => x.State == ModuleState.NotStarted))
            {
                moduleManager.LoadModule(module.ModuleName);
            }
        }
    }
}
