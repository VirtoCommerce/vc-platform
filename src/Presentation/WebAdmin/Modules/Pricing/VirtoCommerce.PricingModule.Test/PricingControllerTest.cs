using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.PricingModule.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Services;
using VirtoCommerce.PricingModule.Web.Controllers.Api;
using VirtoCommerce.PricingModule.Web.Model;

namespace VirtoCommerce.PricingModule.Test
{
	[TestClass]
	public class PricingControllerTest
	{
		[TestMethod]
		public void ChangeProductPrice()
		{
			//Get product price
			var controller = GetController();
			var result = controller.GetProductPrices("v-b005gs3cfg") as OkNegotiatedContentResult<Price[]>;
			var price = result.Content.First();

			//Change product price
			price.Sale += 10;
			var newVal = price.Sale;
			controller.UpdateProductPrices(new Price[] { price });
			result = controller.GetProductPrices("v-b005gs3cfg") as OkNegotiatedContentResult<Price[]>;
			price = result.Content.First();

			Assert.IsTrue(price.Sale == newVal);
		}

		[TestMethod]
		public void ApplyPriceForNewProduct()
		{
			var controller = GetController();
			var result = controller.GetProductPrices("v-b001fa1nuk") as OkNegotiatedContentResult<Price[]>;
			Assert.IsTrue(!result.Content.Any());

			var price = new Price
			{
				 List = 100,
				 Sale = 200,
				 ProductId = "v-b001fa1nuk" ,
				 Currency = CurrencyCodes.USD
 				 
			};
			controller.UpdateProductPrices(new Price[] { price });
			result = controller.GetProductPrices("v-b001fa1nuk") as OkNegotiatedContentResult<Price[]>;
			price = result.Content.First();

		}
		private static PricingController GetController()
		{
			Func<IFoundationPricingRepository> repositoryFactory = () =>
			{
				return new FoundationPricingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			
			var pricingService = new PricingServiceImpl(repositoryFactory);
			var controller = new PricingController(pricingService);
			return controller;
		}
	}
}
