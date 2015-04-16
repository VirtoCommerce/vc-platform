using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Data.Workflow;

namespace VirtoCommerce.OrderModule.Web
{
    public class Module : IModule, IDatabaseModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            //Business logic for core model
            var orderWorkflowService = new ObservableWorkflowService<CustomerOrderStateBasedEvalContext>();

            //Subscribe to order changes. Calculate totals  
            orderWorkflowService.Subscribe(new CalculateTotalsActivity());
            //Adjust inventory activity
            orderWorkflowService.Subscribe(new ObserverFactory<CustomerOrderStateBasedEvalContext>(() => { return new AdjustInventoryActivity(_container.Resolve<IInventoryService>()); }));
            _container.RegisterInstance<IObservable<CustomerOrderStateBasedEvalContext>>(orderWorkflowService);

            _container.RegisterInstance<IWorkflowService>(orderWorkflowService);

            _container.RegisterType<IOrderRepository>(new InjectionFactory(c => new OrderRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor())));
            //_container.RegisterInstance<IInventoryService>(new Mock<IInventoryService>().Object);
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
                var initializer = new SetupDatabaseInitializer<OrderRepositoryImpl, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }

            if (sampleDataLevel == SampleDataLevel.Full)
            {
                using (var db = new EFOrderRepository(_connectionStringName))
                {
                    var initializer = new SqlOrderSampleDatabaseInitializer();
                    initializer.InitializeDatabase(db);
                }
            }
        }

        #endregion
    }
}
