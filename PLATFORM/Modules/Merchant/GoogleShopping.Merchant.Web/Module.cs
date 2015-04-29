using GoogleShopping.MerchantModule.Web.Controllers.Api;
using GoogleShopping.MerchantModule.Web.Helpers.Implementations;
using GoogleShopping.MerchantModule.Web.Helpers.Interfaces;
using GoogleShopping.MerchantModule.Web.Managers;
using GoogleShopping.MerchantModule.Web.Providers;
using GoogleShopping.MerchantModule.Web.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

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

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            var googleMerchantId = (ulong)settingsManager.GetValue(_merchantIdPropertyName, 0);

            var googleShoppingCode = settingsManager.GetValue("GoogleShopping.Merchant.Code", string.Empty);
            var googleShoppingDescription = settingsManager.GetValue("GoogleShopping.Merchant.Description", string.Empty);
            var googleShoppingLogoUrl = settingsManager.GetValue("GoogleShopping.Merchant.LogoUrl", string.Empty);

            var googleShoppingManager = new ShoppingManagerImpl(googleMerchantId, googleShoppingCode, googleShoppingDescription, googleShoppingLogoUrl);

            _container.RegisterInstance<IGoogleContentServiceProvider>(new ServiceGoogleContentServiceProvider());
            _container.RegisterInstance<IDateTimeProvider>(new DefaultDateTimeProvider());
            _container.RegisterInstance<IShopping>(googleShoppingManager);
            _container.RegisterType<IGoogleProductProvider, VCGoogleProductProvider>(new ContainerControlledLifetimeManager());

            _container.RegisterType<GoogleShoppingController>();
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
