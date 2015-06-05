using System;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PayPal.PaymentGatewaysModule.Web.Managers;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Services;
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
			Func<PaypalPaymentMethod> paypalPaymentMethodFactory = () =>
			{
				return new PaypalPaymentMethod()
				{
					Name = "PayPal",
					Description = "PayPal payment integration",
					LogoUrl = "http://virtocommerce.com/Content/images/PayPal.png",
					Settings = _container.Resolve<ISettingsManager>().GetModuleSettings("Paypal.PaymentGateway")
				};
			};

			_container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(paypalPaymentMethodFactory);

			//Func<PaypalBankCardsPaymentMethod> paypalBankCardsPaymentMethodFactory = () =>
			//{
			//	return new PaypalBankCardsPaymentMethod()
			//	{
			//		Name = "PayPal new",
			//		Description = "PayPal payment integration",
			//		LogoUrl = "http://www.credit-card-logos.com/images/multiple_credit-card-logos-2/credit_card_paypal_logos_2.gif",
			//		Settings = _container.Resolve<ISettingsManager>().GetModuleSettings("Paypal.PaymentGateway")
			//	};
			//};

			//_container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(paypalBankCardsPaymentMethodFactory);
		}

		public void PostInitialize()
		{

		}

		#endregion
	}
}
