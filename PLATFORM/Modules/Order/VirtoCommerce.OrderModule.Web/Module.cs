using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Data.Workflow;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.OrderModule.Data.Workflow;
using VirtoCommerce.Domain.Order.Workflow;

namespace VirtoCommerce.OrderModule.Web
{
    public class Module : IModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new OrderRepositoryImpl(_connectionStringName))
            {
				IDatabaseInitializer<OrderRepositoryImpl> initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new OrderSampleDatabaseInitializer();
                        break;
                    default:
						initializer = new SetupDatabaseInitializer<OrderRepositoryImpl, VirtoCommerce.OrderModule.Data.Migrations.Configuration>();
                        break;
                }

                initializer.InitializeDatabase(context);
            }
        }

        public void Initialize()
        {
            //Business logic for core model
			var orderWorkflowService = new CustomerOrderWorkflow();

            //Subscribe to order changes. Calculate totals  
            orderWorkflowService.Subscribe(new CalculateTotalsActivity());
            //Adjust inventory activity
            orderWorkflowService.Subscribe(new ObserverFactory<OrderStateBasedEvalContext>(() => { return new AdjustInventoryActivity(_container.Resolve<IInventoryService>()); }));
			_container.RegisterInstance<IObservable<OrderStateBasedEvalContext>>(orderWorkflowService);

            _container.RegisterInstance<IOrderWorkflow>(orderWorkflowService);

            _container.RegisterType<IOrderRepository>(new InjectionFactory(c => new OrderRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor())));
            //_container.RegisterInstance<IInventoryService>(new Mock<IInventoryService>().Object);
            _container.RegisterType<IOperationNumberGenerator, TimeBasedNumberGeneratorImpl>();

            _container.RegisterType<ICustomerOrderService, CustomerOrderServiceImpl>();
            _container.RegisterType<ICustomerOrderSearchService, CustomerOrderSearchServiceImpl>();
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
