using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.InventoryModule.Data.Repositories;
using VirtoCommerce.InventoryModule.Data.Services;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.InventoryModule.Web
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
            _container.RegisterType<IFoundationInventoryRepository>(new InjectionFactory(c => new FoundationInventoryRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor())));

            _container.RegisterType<IInventoryService, InventoryServiceImpl>();
        }

        #endregion
    }
}
