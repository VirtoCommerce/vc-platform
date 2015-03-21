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
using VirtoCommerce.OrderModule.Data.Interceptors;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.OrderModule.Data.Workflow;
using VirtoCommerce.Foundation.Data.Infrastructure;

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
			var mockInventory = new Mock<IInventoryService>();

			Func<IOrderRepository> orderRepositoryFactory = () => { 
										return new OrderRepositoryImpl("VirtoCommerce", 
										  							 new InventoryOperationInterceptor(mockInventory.Object),
																	 new AuditableInterceptor(),
																	 new EntityPrimaryKeyGeneratorInterceptor());
			};

			//Business logic for core model
			var orderWorkflowService = new ObservableWorkflowService<CustomerOrder>();
			//Subscribe to order changes. Calculate totals  
			orderWorkflowService.Subscribe(new CalculateTotalsActivity());
			_container.RegisterInstance<IObservable<CustomerOrder>>(orderWorkflowService);
			_container.RegisterInstance<IWorkflowService>(orderWorkflowService);
		
			_container.RegisterType<Func<IOrderRepository>>(new InjectionFactory(x => orderRepositoryFactory));
			//_container.RegisterInstance<IInventoryService>(mockInventory.Object);
			_container.RegisterType<IOperationNumberGenerator, TimeBasedNumberGeneratorImpl>();

			_container.RegisterType<ICustomerOrderService, CustomerOrderServiceImpl>();
			_container.RegisterType<ICustomerOrderSearchService, CustomerOrderSearchServiceImpl>();
			
		}

		#endregion

		#region IDatabaseModule Members

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
			using (var context = new OrderRepositoryImpl())
			{
				var initializer = new  SetupDatabaseInitializer<OrderRepositoryImpl, VirtoCommerce.OrderModule.Data.Migrations.Configuration>();
				initializer.InitializeDatabase(context);
			}
		}

		#endregion
	}
}
