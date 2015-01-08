using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.CartModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			var repository = new InMemoryCartRepository();

			var cartService = new ShoppingCartServiceImpl(repository);
			var searchService = new ShoppingCartSearchServiceImpl(repository);

			_container.RegisterType<CartController>(new InjectionConstructor(cartService, searchService));
		}
	}
}