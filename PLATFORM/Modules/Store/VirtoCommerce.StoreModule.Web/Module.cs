using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
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

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
			using (var db = new StoreRepositoryImpl("VirtoCommerce"))
			{
				IDatabaseInitializer<StoreRepositoryImpl> initializer;

				switch (sampleDataLevel)
				{
					case SampleDataLevel.Full:
					case SampleDataLevel.Reduced:
						initializer = new SqlStoreSampleDatabaseInitializer(_container.Resolve<ISettingsManager>());
						break;
					default:
						initializer = new SetupDatabaseInitializer<StoreRepositoryImpl, VirtoCommerce.StoreModule.Data.Migrations.Configuration>();
						break;
				}

				initializer.InitializeDatabase(db);
			}
        }

		public void Initialize()
		{
			_container.RegisterType<IStoreRepository>(new InjectionFactory(c => new StoreRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));
			_container.RegisterType<IStoreService, StoreServiceImpl>();
		}
        public void PostInitialize()
        {
        }

		
        #endregion
    }
}
