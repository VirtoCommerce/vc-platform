using Microsoft.Practices.Unity;
using System.Collections.Generic;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.OrderModule.Data;
using VirtoCommerce.OrderModule.Data.Repositories;
using System;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Domain.Inventory.Services;
using Moq;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.OrderModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Order.Services;

namespace VirtoCommerce.OrderModule.Web
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
			Func<IOrderRepository> orderRepositoryFactory = () => { return new OrderRepositoryImpl("VirtoCommerce"); };
		
			var mockInventory = new Mock<IInventoryService>();

			_container.RegisterType<Func<IOrderRepository>>(new InjectionFactory(x => orderRepositoryFactory));
			_container.RegisterInstance<IInventoryService>(mockInventory.Object);
			_container.RegisterType<IOperationNumberGenerator, TimeBasedNumberGeneratorImpl>();

			_container.RegisterType<ICustomerOrderService, CustomerOrderServiceImpl>();
			_container.RegisterType<ICustomerOrderSearchService, CustomerOrderSearchServiceImpl>();
			
		}

		#endregion

		#region IDatabaseModule Members

		public void SetupDatabase(bool insertSampleData, bool reducedSampleData)
		{
			using (var context = new OrderRepositoryImpl())
			{
				var initializer = new OrderDatabaseInitializer();
				initializer.InitializeDatabase(context);
			}
		}

		#endregion
	}
}
