using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
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
			using (var context = new PricingRepositoryImpl("VirtoCommerce"))
			{
				IDatabaseInitializer<PricingRepositoryImpl> initializer;

				switch (sampleDataLevel)
				{
					case SampleDataLevel.Full:
					case SampleDataLevel.Reduced:
						initializer = new PricingSampleDatabaseInitializer();
						break;
					default:
						initializer = new SetupDatabaseInitializer<PricingRepositoryImpl, VirtoCommerce.PricingModule.Data.Migrations.Configuration>();
						break;
				}

				initializer.InitializeDatabase(context);
			}
        }

        public void Initialize()
        {
			_container.RegisterType<IPricingRepository>(new InjectionFactory(c => new PricingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));
		}

        public void PostInitialize()
        {
        }

        #endregion
    }
}
