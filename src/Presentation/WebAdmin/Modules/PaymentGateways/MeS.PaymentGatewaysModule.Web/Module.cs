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

			var mesAccountId = settingsManager.GetValue("MeS.PaymentGateway.Credentials.AppKey", string.Empty);

			//var paymentGatewayManager = ServiceLocator.Current.GetInstance<IPaymentGatewayManager>();

			var mesPaymentGateway = new MeSPaymentGatewayImpl();

			//paymentGatewayManager.RegisterGateway(mesPaymentGateway);

			_container.RegisterType<MeSGatewayController>(new InjectionConstructor(mesPaymentGateway, mesAccountId));
		}
	}
}