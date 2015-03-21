using MeS.PaymentGatewaysModule.Web.Controllers;
using MeS.PaymentGatewaysModule.Web.Managers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.Domain.Payment.Services;

namespace MeS.PaymentGatewaysModule.Web
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

			var mesAccountId = settingsManager.GetValue("Mes.PaymentGateway.Credentials.AppKey", string.Empty);

			var mesGatewayCode = settingsManager.GetValue("Mes.PaymentGateway.GatewayDescription.GatewayCode", string.Empty);
			var mesDescription = settingsManager.GetValue("Mes.PaymentGateway.GatewayDescription.Description", string.Empty);
			var mesLogoUrl = settingsManager.GetValue("Mes.PaymentGateway.GatewayDescription.LogoUrl", string.Empty);

			var mesPaymentGateway = new MeSPaymentGatewayImpl(mesAccountId, mesGatewayCode, mesDescription, mesLogoUrl);
			var paymentGatewayManager = _container.Resolve<IPaymentGatewayManager>();
			paymentGatewayManager.RegisterGateway(mesPaymentGateway);
		
			_container.RegisterType<MeSGatewayController>(new InjectionConstructor(mesPaymentGateway, mesAccountId));
		}
	}
}