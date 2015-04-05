using GoogleShopping.MerchantModule.Web.Controllers.Api;
using GoogleShopping.MerchantModule.Web.Managers;
using GoogleShopping.MerchantModule.Web.Providers;
using GoogleShopping.MerchantModule.Web.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;

namespace GoogleShopping.MerchantModule.Web
{
    public class Module : IModule
    {
        private const string _merchantIdPropertyName = "GoogleShopping.Merchant.MerchantId";

        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            var googleMerchantId = (ulong) settingsManager.GetValue(_merchantIdPropertyName, 0);

            var googleShoppingCode = settingsManager.GetValue("GoogleShopping.Merchant.Code", string.Empty);
            var googleShoppingDescription = settingsManager.GetValue("GoogleShopping.Merchant.Description", string.Empty);
            var googleShoppingLogoUrl = settingsManager.GetValue("GoogleShopping.Merchant.LogoUrl", string.Empty);

            var googleShoppingManager = new ShoppingManagerImpl(googleMerchantId, googleShoppingCode, googleShoppingDescription, googleShoppingLogoUrl);

            _container.RegisterInstance<IShopping>(googleShoppingManager);
            _container.RegisterType<IGoogleProductProvider, VCGoogleProductProvider>();

            _container.RegisterType<GoogleShoppingController>();
        }
    }
}