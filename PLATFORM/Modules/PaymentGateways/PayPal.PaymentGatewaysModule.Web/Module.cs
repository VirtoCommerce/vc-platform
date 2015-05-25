using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PayPal.PaymentGatewaysModule.Web.Controllers;
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
		
		}

		public void PostInitialize()
		{
			var storeService = ServiceLocator.Current.GetInstance<IStoreService>();
			var customerOrderService = ServiceLocator.Current.GetInstance<ICustomerOrderService>();

			PaypalStoreSettingInitializer initializer = new PaypalStoreSettingInitializer(storeService);
			initializer.Initialize();

			var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

			var paypalGatewayCode = settingsManager.GetValue("Paypal.PaymentGateway.GatewayDescription.GatewayCode", string.Empty);
			var paypalDescription = settingsManager.GetValue("Paypal.PaymentGateway.GatewayDescription.Description", string.Empty);
			var paypalLogoUrl = settingsManager.GetValue("Paypal.PaymentGateway.GatewayDescription.LogoUrl", string.Empty);

			var paypalPaymentMethod = new PaypalPaymentMethod(storeService, customerOrderService);
			paypalPaymentMethod.Description = paypalDescription;
			paypalPaymentMethod.LogoUrl = paypalPaymentMethod.LogoUrl;

			var paymentMethodsService = _container.Resolve<IPaymentMethodsService>();
			paymentMethodsService.RegisterPaymentMethod(() => paypalPaymentMethod);

			_container.RegisterInstance<PaypalPaymentMethod>(paypalPaymentMethod);
		}

		#endregion
	}
}
