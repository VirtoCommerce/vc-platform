using Klarna.Checkout.Euro.Managers;
using Klarna.Checkout.Euro.Resources;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Klarna.Checkout.Euro
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void Initialize()
        {
            var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

			Func<KlarnaCheckoutEuroPaymentMethod> klarnaCheckoutEuroPaymentMethodFactory = () => new KlarnaCheckoutEuroPaymentMethod
            {
                Name = KlarnaCheckoutEuroResource.PaymentMethodName,
				Description = KlarnaCheckoutEuroResource.PaymentMethodDescription,
                LogoUrl = KlarnaCheckoutEuroResource.PaymentMethodLogoUrl,
				Settings = settingsManager.GetModuleSettings("Klarna.Checkout.Euro")
            };

            var paymentMethodsService = _container.Resolve<IPaymentMethodsService>();
			paymentMethodsService.RegisterPaymentMethod(klarnaCheckoutEuroPaymentMethodFactory);
        }

        #endregion
    }
}
