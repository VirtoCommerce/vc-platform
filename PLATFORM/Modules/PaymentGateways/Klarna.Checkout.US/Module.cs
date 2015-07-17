using Klarna.Checkout.US.Managers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Klarna.Checkout.US
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
			//var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

			//Func<KlarnaCheckoutUSPaymentMethod> klarnaPaymentMethodFactory = () => new KlarnaCheckoutUSPaymentMethod
			//{
			//	Name = "Klarna Checkout US/UK",
			//	Description = "Klarna Checkout euro payment integration",
			//	LogoUrl = "http://virtocommerce.com/Content/images/Blue-black.png",
			//	Settings = settingsManager.GetModuleSettings("Klarna.Checkout.US")
			//};

			//var paymentMethodsService = _container.Resolve<IPaymentMethodsService>();
			//paymentMethodsService.RegisterPaymentMethod(klarnaPaymentMethodFactory);
        }

        #endregion
    }
}
