using Microsoft.Practices.Unity;
using System.Web.Hosting;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.PackagingModule.Data.Services;
using VirtoCommerce.PackagingModule.Web.Controllers.Api;

namespace VirtoCommerce.PackagingModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            var packageService = new PackageService(HostingEnvironment.MapPath("~/Content/Packages"), HostingEnvironment.MapPath("~/Modules"), HostingEnvironment.MapPath("~/Packages"));
            _container.RegisterType<ModulesController>(new InjectionConstructor(packageService, "Content/Packages"));
        }

        #endregion
    }
}
