using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.InventoryModule.Data.Repositories;
using VirtoCommerce.InventoryModule.Data.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

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

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
			using (var context = new InventoryRepositoryImpl())
			{
				var initializer = new SetupDatabaseInitializer<InventoryRepositoryImpl, VirtoCommerce.InventoryModule.Data.Migrations.Configuration>();
				initializer.InitializeDatabase(context);
			}
        }

        public void Initialize()
        {
			_container.RegisterType<IInventoryRepository>(new InjectionFactory(c => new InventoryRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));

            _container.RegisterType<IInventoryService, InventoryServiceImpl>();
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
