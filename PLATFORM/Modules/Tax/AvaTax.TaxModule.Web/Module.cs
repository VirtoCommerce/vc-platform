using System;
using System.Configuration;
using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Observers;
using AvaTax.TaxModule.Web.Services;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using System.Diagnostics.Tracing;
using AvaTax.TaxModule.Web.Logging;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Asset;

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
            var eventListener = new ObservableEventListener();

            eventListener.EnableEvents(
                VirtoCommerceEventSource.Log,
                EventLevel.LogAlways,
                Keywords.All);

            

            var assetsConnection = ConfigurationManager.ConnectionStrings["AssetsConnectionString"];

            if (assetsConnection != null)
            {
                var properties = assetsConnection.ConnectionString.ToDictionary(";", "=");
                var provider = properties["provider"];
                var assetsConnectionString = properties.ToString(";", "=", "provider");

                if (string.Equals(provider, FileSystemBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    eventListener.LogToRollingFlatFile("AvaTax.log",
                        10000,
                        "hh",
                        RollFileExistsBehavior.Increment,
                        RollInterval.Day);
                }
                else
                    if (string.Equals(provider, AzureBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                    {
                        eventListener.LogToWindowsAzureTable(
                            "VirtoCommerce2", assetsConnectionString);

                    }
            }


            var settingsManager = _container.Resolve<ISettingsManager>();
            
            var avalaraTax = new AvaTaxSettings(_usernamePropertyName, _passwordPropertyName, _serviceUrlPropertyName, _companyCodePropertyName, _isEnabledPropertyName, _isValidateAddressPropertyName, settingsManager);
            
            _container.RegisterType<AvaTaxController>
                (new InjectionConstructor(
                    avalaraTax));

            _container.RegisterInstance<ITaxSettings>(avalaraTax);

            //Subscribe to cart changes. Calculate taxes   
            _container.RegisterType<IObserver<CartChangeEvent>, CalculateCartTaxesObserver>("CalculateCartTaxesObserver");

            //Subscribe to order changes. Calculate taxes   
            _container.RegisterType<IObserver<OrderChangeEvent>, CalculateOrderTaxesObserver>("CalculateOrderTaxesObserver");

            //Subscribe to order changes. Calculate taxes   
            _container.RegisterType<IObserver<OrderChangeEvent>, CancelOrderTaxesObserver>("CancelOrderTaxesObserver");

            //Subscribe to order changes. Adjust taxes   
            _container.RegisterType<IObserver<OrderChangeEvent>, CalculateTaxAdjustmentObserver>("CalculateTaxAdjustmentObserver");
        }
        
        #endregion
    }
}