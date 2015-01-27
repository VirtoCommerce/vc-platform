using System.Collections.Generic;
using Microsoft.Practices.Unity;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Money;
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

		#region IModule Members

		public void Initialize()
		{
			var repository = new InMemoryCartRepository();
			var cart1 = new ShoppingCart
			{
				Id = "cart1",
				Currency = CurrencyCodes.USD,
				SiteId = "site",
				Name = "default",
				Items = new List<CartItem>(new[] { new CartItem { Name = "product 1", ListPrice = 9.99m, PlacedPrice = 9.45m, SalePrice = 9.09m, Quantity = 12 } ,
                                                   new CartItem { Name = "product 2", ListPrice = 9.99m, PlacedPrice = 9.45m, SalePrice = 9.09m, Quantity = 2 } }),
				CustomerId = "customer1",
				CustomerName = "customer name"
			};
			var cart2 = new ShoppingCart
			{
				Id = "cart2",
				Currency = CurrencyCodes.USD,
				SiteId = "site",
				Name = "default",
				Items = new List<CartItem>(new[] { new CartItem { Name = "product 1", ListPrice = 9.99m, PlacedPrice = 9.45m, SalePrice = 9.09m, Quantity = 2 } ,
                                                   new CartItem { Name = "product 2", ListPrice = 90.99m, PlacedPrice = 9.45m, SalePrice = 9.09m, Quantity = 1 } ,
                                                   new CartItem { Name = "product 3", ListPrice = 19.99m, PlacedPrice = 9.45m, SalePrice = 9.09m, Quantity = 2 } }),
				CustomerId = "customer2",
				CustomerName = "customer2 name"
			};
			repository.Add(cart1);
			repository.Add(cart2);
			var cartService = new ShoppingCartServiceImpl(repository);
			var searchService = new ShoppingCartSearchServiceImpl(repository);

			_container.RegisterType<CartController>(new InjectionConstructor(cartService, searchService));
		}

		#endregion
	}
}
