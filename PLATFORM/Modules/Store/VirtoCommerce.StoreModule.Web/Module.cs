using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.StoreModule.Data.Repositories;
using VirtoCommerce.StoreModule.Data.Services;

namespace VirtoCommerce.StoreModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            _container.RegisterType<IFoundationStoreRepository>(new InjectionFactory(c => new FoundationStoreRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor())));

            _container.RegisterType<IStoreService, StoreServiceImpl>();
        }

        #endregion
    }
}
