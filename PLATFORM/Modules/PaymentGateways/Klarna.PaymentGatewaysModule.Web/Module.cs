using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;

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
            //var settingsManager = ServiceLocator.Current.GetInstance<ISettingsManager>();

            //Func<KlarnaPaymentMethod> klarnaPaymentMethodFactory = () =>
            //{
            //	return new KlarnaPaymentMethod()
            //	{
            //		Name = "Klarna",
            //		Description = "Klarna payment integration",
            //		LogoUrl = "https://cdn.klarna.com/1.0/shared/image/generic/logo/nb_no/basic/blue-black.png?width=200",
            //		Settings = settingsManager.GetModuleSettings("Klarna.PaymentGateway")
            //	};
            //};
            //var paymentMethodsService = _container.Resolve<IPaymentMethodsService>();
            //paymentMethodsService.RegisterPaymentMethod(klarnaPaymentMethodFactory);
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
