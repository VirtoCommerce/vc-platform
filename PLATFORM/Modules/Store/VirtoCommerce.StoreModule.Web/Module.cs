using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Modularity;
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
			Func<IFoundationStoreRepository> storeRepositoryFactory = () =>
			{
				return new FoundationStoreRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			_container.RegisterType<Func<IFoundationStoreRepository>>(new InjectionFactory(x => storeRepositoryFactory));

			_container.RegisterType<IStoreService, StoreServiceImpl>();
		}

		#endregion
	}
}