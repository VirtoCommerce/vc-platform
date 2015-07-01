using System;
using Microsoft.Practices.Unity;
using Paypal.ExpressCheckout.Managers;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Paypal.ExpressCheckout
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
            var settings = _container.Resolve<ISettingsManager>().GetModuleSettings("Paypal.ExpressCheckout");

            Func<PaypalExpressCheckoutPaymentMethod> paypalBankCardsExpressCheckoutPaymentMethodFactory = () => new PaypalExpressCheckoutPaymentMethod
            {
                Name = "Paypal Express Checkout",
                Description = "PayPal express checkout integration",
                LogoUrl = "http://www.credit-card-logos.com/images/multiple_credit-card-logos-2/credit_card_paypal_logos_2.gif",
                Settings = settings
            };

            _container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(paypalBankCardsExpressCheckoutPaymentMethodFactory);
        }

        #endregion
    }
}
