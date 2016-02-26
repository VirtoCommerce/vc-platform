using System;
using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Observers;
using AvaTax.TaxModule.Web.Services;
using Common.Logging;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Domain.Tax.Services;
using VirtoCommerce.Domain.Customer.Services;

namespace AvaTax.TaxModule.Web
{
    public class Module: ModuleBase
    {
        private const string _usernamePropertyName = "Avalara.Tax.Credentials.AccountNumber";
        private const string _passwordPropertyName = "Avalara.Tax.Credentials.LicenseKey";
        private const string _serviceUrlPropertyName = "Avalara.Tax.Credentials.ServiceUrl";
        private const string _companyCodePropertyName = "Avalara.Tax.Credentials.CompanyCode";
        private const string _isEnabledPropertyName = "Avalara.Tax.IsEnabled";
        private const string _isValidateAddressPropertyName = "Avalara.Tax.IsValidateAddress";

        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members
        
        public override void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            
            var avalaraTax = new AvaTaxSettings(_usernamePropertyName, _passwordPropertyName, _serviceUrlPropertyName, _companyCodePropertyName, _isEnabledPropertyName, _isValidateAddressPropertyName, settingsManager);

            var logManager = _container.Resolve<ILog>();

            _container.RegisterType<AvaTaxController>(new InjectionConstructor(avalaraTax, logManager));

            _container.RegisterInstance<ITaxSettings>(avalaraTax);

            //Subscribe to cart changes. Calculate taxes   
           // _container.RegisterType<IObserver<CartChangeEvent>, CalculateCartTaxesObserver>("CalculateCartTaxesObserver");

            //Subscribe to order changes. Calculate taxes   
            //_container.RegisterType<IObserver<OrderChangeEvent>, CalculateOrderTaxesObserver>("CalculateOrderTaxesObserver");

            //Subscribe to order changes. Calculate taxes   
            //_container.RegisterType<IObserver<OrderChangeEvent>, CancelOrderTaxesObserver>("CancelOrderTaxesObserver");

            //Subscribe to order changes. Adjust taxes   
           // _container.RegisterType<IObserver<OrderChangeEvent>, CalculateTaxAdjustmentObserver>("CalculateTaxAdjustmentObserver");
        }

        public override void PostInitialize()
        {
            var settingManager = _container.Resolve<ISettingsManager>();
            var taxService = _container.Resolve<ITaxService>();
            var moduleSettings = settingManager.GetModuleSettings("Avalara.Tax");
            taxService.RegisterTaxProvider(() => new AvaTaxRateProvider(_container.Resolve<IContactService>(), _container.Resolve<ILog>(), moduleSettings)
            {
                Name = "Avalara taxes",
                Description = "Avalara service integration",
                LogoUrl = "Modules/$(Avalara.Tax)/Content/400.png"
            });

         
        }
        #endregion
    }
}