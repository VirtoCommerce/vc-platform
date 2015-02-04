using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.CartModule.Web
{
    using VirtoCommerce.CartModule.Data;

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
			Func<ICartRepository> cartRepositoryFactory = () =>
			{
				return new CartRepositoryImpl("VirtoCommerce", new AuditableInterceptor());
			};

			_container.RegisterType<Func<ICartRepository>>(new InjectionFactory(x => cartRepositoryFactory));
		
			_container.RegisterType<IShoppingCartService, ShoppingCartServiceImpl>();
			_container.RegisterType<IShoppingCartSearchService, ShoppingCartSearchServiceImpl>();

		}

		#endregion

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new CartRepositoryImpl())
            {
                var initializer = new CartDatabaseInitializer();
                initializer.InitializeDatabase(context);
            }
        }
	}
}
