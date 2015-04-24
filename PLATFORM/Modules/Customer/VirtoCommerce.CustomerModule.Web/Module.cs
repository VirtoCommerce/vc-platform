using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CustomerModule.Web
{
	public class Module : IModule, IDatabaseModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
			_container.RegisterType<ICustomerRepository>(new InjectionFactory(c => new CustomerRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));

            _container.RegisterType<IOrganizationService, OrganizationServiceImpl>();
            _container.RegisterType<IContactService, ContactServiceImpl>();
            _container.RegisterType<ICustomerSearchService, CustomerSearchServiceImpl>();
        }

        #endregion

		#region IDatabaseModule Members

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
			using (var context = new CustomerRepositoryImpl())
			{
				var initializer = new SetupDatabaseInitializer<CustomerRepositoryImpl, VirtoCommerce.CustomerModule.Data.Migrations.Configuration>();
				initializer.InitializeDatabase(context);
			}

		}

		#endregion
	}
}
