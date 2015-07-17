using System;
using Microsoft.Practices.Unity;
using Paypal.AdaptivePayments.Managers;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Paypal.AdaptivePayments
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
			//var settings = _container.Resolve<ISettingsManager>().GetModuleSettings("Paypal.AdaptivePayments");

			//Func<PaypalAdaptivePaymentsPaymentMethod> paypalBankCardsAdaptivePaymentsPaymentMethodFactory = () => new PaypalAdaptivePaymentsPaymentMethod
			//{
			//	Name = "Paypal Adaptive Payments",
			//	Description = "Paypal adaptive payments integration",
			//	LogoUrl = "http://www.credit-card-logos.com/images/multiple_credit-card-logos-2/credit_card_paypal_logos_2.gif",
			//	Settings = settings
			//};

			//_container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(paypalBankCardsAdaptivePaymentsPaymentMethodFactory);
        }

        #endregion
    }
}
