using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PayPal.PaymentGatewaysModule.Web.Controllers;
using PayPal.PaymentGatewaysModule.Web.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.Domain.Payment.Services;

namespace PayPal.PaymentGatewaysModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

			var paypalAppId = settingsManager.GetValue("Paypal.PaymentGateway.Credentials.AppKey", string.Empty);
			var paypalSecret = settingsManager.GetValue("Paypal.PaymentGateway.Credentials.Secret", string.Empty);

			var paypalGatewayCode = settingsManager.GetValue("Paypal.PaymentGateway.GatewayDescription.GatewayCode", string.Empty);
			var paypalDescription = settingsManager.GetValue("Paypal.PaymentGateway.GatewayDescription.Description", string.Empty);
			var paypalLogoUrl = settingsManager.GetValue("Paypal.PaymentGateway.GatewayDescription.LogoUrl", string.Empty);

			var paypalPaymentGateway = new PayPalPaymentGatewayImpl(paypalAppId, paypalSecret, paypalGatewayCode, paypalDescription, paypalLogoUrl);
			var paymentGatewayManager = _container.Resolve<IPaymentGatewayManager>();
			paymentGatewayManager.RegisterGateway(paypalPaymentGateway);

			_container.RegisterType<PayPalGatewayController>(new InjectionConstructor(paypalPaymentGateway, paypalAppId, paypalSecret));
		}
	}
}