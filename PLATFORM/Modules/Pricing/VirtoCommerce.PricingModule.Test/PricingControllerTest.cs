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
		public void GetProductPriceInfo()
		{	
			//Get product price lists
			var controller = GetController();
			var result = controller.GetProductPriceLists("v-b005gs3cfg") as OkNegotiatedContentResult<Pricelist[]>;
			var pricelists = result.Content;

			var allPricesCurrencyGroup = pricelists.SelectMany(x=>x.Prices).GroupBy(x=>x.Currency);
			var allPricesWithSpeciffiedCurrency = allPricesCurrencyGroup.Where(x=>x.Any()).FirstOrDefault();
			var minPrice = allPricesWithSpeciffiedCurrency.Min(x => x.List);
			var maxPrice = allPricesWithSpeciffiedCurrency.Max(x => x.List);

			var resultLabel = minPrice.ToString();
			if(maxPrice > minPrice)
			{
				resultLabel += " - " + maxPrice;
			}
			resultLabel += allPricesWithSpeciffiedCurrency.Key;
		}

		[TestMethod]
		public void ChangeProductPrice()
		{
			//Get product price lists
			var controller = GetController();
			var result = controller.GetProductPriceLists("v-b005gs3cfg") as OkNegotiatedContentResult<Pricelist[]>;
			var pricelists = result.Content;

			var priceList = pricelists.Where(x => x.Prices.Any()).FirstOrDefault();

			var changePrice = priceList.Prices.FirstOrDefault();
			changePrice.Sale += 22.44m;

			var newPrice = new Price
			{
				ProductId = "v-b005gs3cfg",
				List = 12.22m,
				Sale = 22.11m
			};
			priceList.Prices.Add(newPrice);


			controller.UpdateProductPriceLists("v-b005gs3cfg", priceList);

			result = controller.GetProductPriceLists("v-b005gs3cfg") as OkNegotiatedContentResult<Pricelist[]>;
			priceList = pricelists.FirstOrDefault(x => x.Id == priceList.Id);

		
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
