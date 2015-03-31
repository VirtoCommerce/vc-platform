using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CartModule.Data.Workflow;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.CartModule.Web
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
            //Business logic for core model
            var cartWorkflowService = new ObservableWorkflowService<ShoppingCart>();
            //Subscribe to cart changes. Calculate totals  
            cartWorkflowService.Subscribe(new CalculateTotalsActivity());
            _container.RegisterInstance<IObservable<ShoppingCart>>(cartWorkflowService);

            _container.RegisterType<ICartRepository>(new InjectionFactory(c => new CartRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor())));

            _container.RegisterType<IShoppingCartService, ShoppingCartServiceImpl>();
            _container.RegisterType<IShoppingCartSearchService, ShoppingCartSearchServiceImpl>();
        }

        #endregion

        #region IDatabaseModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new CartRepositoryImpl())
            {
                var initializer = new SetupDatabaseInitializer<CartRepositoryImpl, VirtoCommerce.CartModule.Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }

        }

        #endregion
    }
}
