using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Managers;
using AvaTax.TaxModule.Web.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web
{
    public class Module: ModuleBase
    {
        private const string _usernamePropertyName = "Avalara.Tax.Credentials.AccountNumber";
        private const string _passwordPropertyName = "Avalara.Tax.Credentials.LicenseKey";
        private const string _serviceUrlPropertyName = "Avalara.Tax.Credentials.ServiceUrl";
        private const string _companyCodePropertyName = "Avalara.Tax.Credentials.CompanyCode";

        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members
        
        public override void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            var avalaraUsername = settingsManager.GetValue(_usernamePropertyName, string.Empty);
            var avalaraPassword = settingsManager.GetValue(_passwordPropertyName, string.Empty);
            var avalaraServiceUrl = settingsManager.GetValue(_serviceUrlPropertyName, string.Empty);
            var avalaraCompanyCode = settingsManager.GetValue(_companyCodePropertyName, string.Empty);

            var avalaraCode = settingsManager.GetValue("Avalara.Tax.Code", string.Empty);
            var avalaraDescription = settingsManager.GetValue("Avalara.Tax.Description", string.Empty);
            var avalaraLogoUrl = settingsManager.GetValue("Avalara.Tax.LogoUrl", string.Empty);


            var avalaraTax = new AvaTaxImpl(avalaraUsername, avalaraPassword, avalaraServiceUrl, avalaraCompanyCode, avalaraCode, avalaraDescription, avalaraLogoUrl);

            #region Avalara manager
            _container.RegisterInstance<ITaxManager>(new InMemoryTaxManagerImpl());
            #endregion

            var avalaraManager = _container.Resolve<ITaxManager>();
            avalaraManager.RegisterTax(avalaraTax);

            _container.RegisterType<AvaTaxController>
                (new InjectionConstructor(
                    avalaraTax));
        }
        
        #endregion
    }
}