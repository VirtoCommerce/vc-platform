using Klarna.PaymentGatewaysModule.Web.Controllers;
using Klarna.PaymentGatewaysModule.Web.Managers;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Klarna.PaymentGatewaysModule.Web
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
			//var settingsManager = _container.Resolve<ISettingsManager>();

			//var klarnaEid = settingsManager.GetValue("Klarna.PaymentGateway.Credentials.AppKey", 0);
			//var klarnaSecret = settingsManager.GetValue("Klarna.PaymentGateway.Credentials.SecretKey", string.Empty);

			//var klarnaGatewayCode = settingsManager.GetValue("Klarna.PaymentGateway.GatewayDescription.GatewayCode", string.Empty);
			//var klarnaDescription = settingsManager.GetValue("Klarna.PaymentGateway.GatewayDescription.Description", string.Empty);
			//var klarnaLogoUrl = settingsManager.GetValue("Klarna.PaymentGateway.GatewayDescription.LogoUrl", string.Empty);


			//var klarnaPaymentGateway = new KlarnaPaymentGatewayImpl(klarnaEid, klarnaSecret, klarnaGatewayCode, klarnaDescription, klarnaLogoUrl);
			//var paymentGatewayManager = _container.Resolve<IPaymentGatewayManager>();
			//paymentGatewayManager.RegisterGateway(klarnaPaymentGateway);

			//_container.RegisterType<KlarnaGatewayController>(new InjectionConstructor(klarnaPaymentGateway, klarnaEid, klarnaSecret));
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
