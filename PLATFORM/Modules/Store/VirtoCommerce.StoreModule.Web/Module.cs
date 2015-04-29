using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.StoreModule.Data.Repositories;
using VirtoCommerce.StoreModule.Data.Services;
using VirtoCommerce.StoreModule.Web.SampleData;

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
				SqlStoreDatabaseInitializer initializer;

				switch (sampleDataLevel)
				{
					case SampleDataLevel.Full:
					case SampleDataLevel.Reduced:
						initializer = new SqlStoreSampleDatabaseInitializer();
						break;
					default:
						initializer = new SqlStoreDatabaseInitializer();
						break;
				}

				initializer.InitializeDatabase(db);
			}
        }

		public void Initialize()
		{
			_container.RegisterType<IStoreRepository>(new InjectionFactory(c => new StoreRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));
		}
        public void PostInitialize()
        {
        }

		
        #endregion
    }
}
