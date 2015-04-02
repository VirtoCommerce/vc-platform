using GoogleShopping.MerchantModule.Web.Controllers.Api;
using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;

namespace GoogleShopping.MerchantModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            _container.RegisterType<GAuthorizationController>
                (new InjectionConstructor(
                    settingsManager));
        }
    }
}