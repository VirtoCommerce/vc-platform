using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
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

		}

		[TestMethod]
		public void ChangeProductPrice()
		{
			//Get product price lists
			//var controller = GetController();
			//var result = controller.GetProductPriceLists("v-b005gs3cfg") as OkNegotiatedContentResult<Pricelist[]>;
			//var pricelists = result.Content;

			//var priceList = pricelists.Where(x => x.ProductPrices.Any()).FirstOrDefault();

			//var changePrice = priceList.ProductPrices.FirstOrDefault();
			//changePrice.MaxPrice.Sale += 22.44m;

			//var newPrice = new Price
			//{
			//	ProductId = "v-b005gs3cfg",
			//	List = 12.22m,
			//	Sale = 22.11m
			//};
			//priceList.Prices.Add(newPrice);


			//controller.UpdateProductPriceLists("v-b005gs3cfg", priceList);

			//result = controller.GetProductPriceLists("v-b005gs3cfg") as OkNegotiatedContentResult<Pricelist[]>;
			//priceList = pricelists.FirstOrDefault(x => x.Id == priceList.Id);

		
		}

		private static PricingController GetController()
		{
			Func<IPricingRepository> repositoryFactory = () =>
			{
				return new PricingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
			};

			var pricingService = new PricingServiceImpl(repositoryFactory);
			var controller = new PricingController(pricingService, null, null);
			return controller;
		}
	}
}
