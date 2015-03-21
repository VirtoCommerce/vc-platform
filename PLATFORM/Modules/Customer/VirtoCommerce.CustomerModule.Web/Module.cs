using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Customer.Services;

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
			Func<IFoundationCustomerRepository> customerRepositoryFactory = () =>
			{
				return new FoundationCustomerRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			_container.RegisterType<Func<IFoundationCustomerRepository>>(new InjectionFactory(x => customerRepositoryFactory));

			_container.RegisterType<IOrganizationService, OrganizationServiceImpl>();
			_container.RegisterType<IContactService, ContactServiceImpl>();
			_container.RegisterType<ICustomerSearchService, CustomerSearchServiceImpl>();
		}

		#endregion
	}
}