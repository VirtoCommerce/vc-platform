using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.CustomerModule.Web
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
            _container.RegisterType<IFoundationCustomerRepository>(new InjectionFactory(c => new FoundationCustomerRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor())));

            _container.RegisterType<IOrganizationService, OrganizationServiceImpl>();
            _container.RegisterType<IContactService, ContactServiceImpl>();
            _container.RegisterType<ICustomerSearchService, CustomerSearchServiceImpl>();
        }

        #endregion
    }
}
