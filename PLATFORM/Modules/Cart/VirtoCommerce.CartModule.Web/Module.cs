using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.CartModule.Data.Observers;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Common.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CartModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new CartRepositoryImpl())
            {
                var initializer = new SetupDatabaseInitializer<CartRepositoryImpl, VirtoCommerce.CartModule.Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }

        }

        public void Initialize()
        {
			_container.RegisterType<IEventPublisher<CartChangeEvent>, EventPublisher<CartChangeEvent>>();

			//Subscribe to cart changes. Calculate totals  
			_container.RegisterType<IObserver<CartChangeEvent>, CalculateCartTotalsObserver>("CalculateCartTotalsObserver");

			_container.RegisterType<ICartRepository>(new InjectionFactory(c => new CartRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));

            _container.RegisterType<IShoppingCartService, ShoppingCartServiceImpl>();
            _container.RegisterType<IShoppingCartSearchService, ShoppingCartSearchServiceImpl>();
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
