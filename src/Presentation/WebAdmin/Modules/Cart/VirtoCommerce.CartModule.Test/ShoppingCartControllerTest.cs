using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Cart.Repositories;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CartModule.Test
{
	[TestClass]
	public class ShoppingCartControllerTest
	{
		[TestMethod]
		public void GetCurrentCartTest()
		{
			var controller = GetCartController();
			var result = controller.GetCurrentCart("testSite") as OkNegotiatedContentResult<webModel.ShoppingCart>;
			Assert.IsNotNull(result.Content);
		}

		[TestMethod]
		public void AddItemToShoppingCart()
		{
			var controller = GetCartController();
			var result = controller.GetCurrentCart("testSite") as OkNegotiatedContentResult<webModel.ShoppingCart>;
			var cart = result.Content;

			var item = new webModel.CartItem
			{
				CatalogId = "catalog",
				CategoryId = "category",
				ListPrice = 10,
				PlacedPrice = 20,
				Quantity = 2,
				Name = "shoes",
				Currency = cart.Currency
			};
			cart.Items = new List<webModel.CartItem>();
			cart.Items.Add(item);

			controller.Update(cart);

			result = controller.GetCartById(cart.Id) as OkNegotiatedContentResult<webModel.ShoppingCart>;
			cart = result.Content;


			Assert.IsNotNull(result.Content);
		}

		[TestMethod]
		public void SearchCarts()
		{
			var controller = GetCartController();
			var result = controller.SearchCarts(new webModel.SearchCriteria { SiteId = "test" }) as OkNegotiatedContentResult<webModel.SearchResult>;
			Assert.IsNotNull(result.Content);
		}

		private static CartController GetCartController()
		{
			var repository = GetRepository();
			var controller = new CartController(GetShoppingCartService(repository), GetShoppingCartSearchService(repository));
			return controller;
		}

		private static IShoppingCartSearchService GetShoppingCartSearchService(IShoppingCartRepository repository)
		{
			var retVal = new ShoppingCartSearchServiceImpl(repository);
			return retVal;
		}

		private static IShoppingCartService GetShoppingCartService(IShoppingCartRepository repository)
		{
			var retVal = new ShoppingCartServiceImpl(repository);
			return retVal;
		}

		private static IShoppingCartRepository GetRepository()
		{
			var retVal = new InMemoryCartRepository();
			return retVal;
		}
	}
}
