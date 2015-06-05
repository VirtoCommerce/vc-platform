using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Managers;
using AvaTax.TaxModule.Web.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web
{
    public class Module: IModule
    {
        private const string _usernamePropertyName = "Avalara.Tax.Credentials.Username";
        private const string _passwordPropertyName = "Avalara.Tax.Credentials.Password";

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

            var avalaraUsername = settingsManager.GetValue(_usernamePropertyName, string.Empty);
            var avalaraPassword = settingsManager.GetValue(_passwordPropertyName, string.Empty);

            var avalaraCode = settingsManager.GetValue("Avalara.Tax.Code", string.Empty);
            var avalaraDescription = settingsManager.GetValue("Avalara.Tax.Description", string.Empty);
            var avalaraLogoUrl = settingsManager.GetValue("Avalara.Tax.LogoUrl", string.Empty);


            var avalaraTax = new AvaTaxImpl(avalaraUsername, avalaraPassword, avalaraCode, avalaraDescription, avalaraLogoUrl);

            #region Avalara manager
            _container.RegisterInstance<ITaxManager>(new InMemoryTaxManagerImpl());
            #endregion

            var avalaraManager = _container.Resolve<ITaxManager>();
            avalaraManager.RegisterTax(avalaraTax);

            _container.RegisterType<AvaTaxController>
                (new InjectionConstructor(
                    avalaraTax));

            _container.RegisterType<AvalaraAuthorizationController>
                (new InjectionConstructor(
                    settingsManager));
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}