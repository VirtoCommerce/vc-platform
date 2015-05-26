using System;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PayPal.PaymentGatewaysModule.Web.Managers;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Order.Workflow;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace PayPal.PaymentGatewaysModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;

		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{

		}

		public void Initialize()
		{
			var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

			Func<PaypalPaymentMethod> paypalPaymentMethodFactory = () =>
			{
				return new PaypalPaymentMethod()
				{
					Name = "PayPal",
					Description = "PayPal payment integration",
					LogoUrl = "https://www.paypalobjects.com/webstatic/mktg/logo/pp_cc_mark_37x23.jpg",
					Settings = settingsManager.GetModuleSettings("Paypal.PaymentGateway")
				};
			};
			var paymentMethodsService = _container.Resolve<IPaymentMethodsService>();
			paymentMethodsService.RegisterPaymentMethod(paypalPaymentMethodFactory);
		}

		public void PostInitialize()
		{

		}

		#endregion
	}
}
