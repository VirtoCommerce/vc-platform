using System;
using Microsoft.Practices.Unity;
using Paypal.DirectPayments.Managers;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Paypal.DirectPayments
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
			//var settings = _container.Resolve<ISettingsManager>().GetModuleSettings("Paypal.DirectPayments");

			//Func<PaypalDirectPaymentsPaymentMethod> paypalBankCardsExpressCheckoutPaymentMethodFactory = () => new PaypalDirectPaymentsPaymentMethod
			//{
			//	Name = "Credit Card",
			//	Description = "Paypal direct payment integration",
			//	LogoUrl = "http://www.credit-card-logos.com/images/multiple_credit-card-logos-2/credit_card_paypal_logos_2.gif",
			//	Settings = settings
			//};

			//_container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(paypalBankCardsExpressCheckoutPaymentMethodFactory);
        }

        #endregion
    }
}
