using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.PricingModule.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Services;

namespace VirtoCommerce.PricingModule.Web
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
            _container.RegisterType<IFoundationPricingRepository>(new InjectionFactory(c => new FoundationPricingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor())));
            _container.RegisterType<IPricingService, PricingServiceImpl>();
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
