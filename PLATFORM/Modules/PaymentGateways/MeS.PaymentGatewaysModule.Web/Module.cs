using MeS.PaymentGatewaysModule.Web.Controllers;
using MeS.PaymentGatewaysModule.Web.Managers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace MeS.PaymentGatewaysModule.Web
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
			var settings = _container.Resolve<ISettingsManager>().GetModuleSettings("MeS.PaymentGateway");

			Func<MesPaymentMethod> meSPaymentMethodFactory = () =>
			{
				return new MesPaymentMethod()
				{
					Name = "Merchant e-solutions payment gateway",
					Description = "Merchant e-solutions payment gateway integration",
					LogoUrl = "http://www.ebs-next.com/images/partners/partners-merchsolutions.jpg",
					Settings = settings
				};
			};

			_container.Resolve<IPaymentMethodsService>().RegisterPaymentMethod(meSPaymentMethodFactory);
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
