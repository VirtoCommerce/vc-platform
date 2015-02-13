using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.InventoryModule.Data.Repositories;
using VirtoCommerce.InventoryModule.Data.Services;

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
			Func<IFoundationInventoryRepository> repositoryFactory = () =>
			{
				return new FoundationInventoryRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			_container.RegisterType<Func<IFoundationInventoryRepository>>(new InjectionFactory(x => repositoryFactory));

			_container.RegisterType<IInventoryService, InventoryServiceImpl>();
			
		}

		#endregion

		
	}
}
