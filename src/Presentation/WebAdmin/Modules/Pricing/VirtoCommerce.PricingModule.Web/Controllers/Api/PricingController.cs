using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.PricingModule.Web.Converters;
using webModel = VirtoCommerce.PricingModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.PricingModule.Web.Controllers.Api
{
	[RoutePrefix("")]
	public class PricingController : ApiController
	{
		private readonly IPricingService _pricingService;
		public PricingController(IPricingService pricingService)
		{
			_pricingService = pricingService;
		}

		// GET: api/products/{productId}/prices
		[HttpGet]
		[ResponseType(typeof(webModel.Price[]))]
		[Route("api/products/{productId}/prices")]
		public IHttpActionResult GetProductPrices(string productId)
		{
			IHttpActionResult retVal = NotFound();
			var prices = _pricingService.EvaluateProductPrices(new coreModel.PriceEvaluationContext { ProductId = productId });
			if (prices != null)
			{
				retVal = Ok(prices.Select(x => x.ToWebModel()).ToArray());
			}
			return retVal;
		}

		// GET: api/catalog/products/{productId}/pricelists
		[HttpGet]
		[ResponseType(typeof(webModel.Pricelist[]))]
		[Route("api/catalog/products/{productId}/pricelists")]
		public IHttpActionResult GetProductPriceLists(string productId)
		{
			var result = new List<webModel.Pricelist>();
			IHttpActionResult retVal = NotFound();
			var priceLists = _pricingService.GetPriceLists();
			foreach (var priceList in priceLists)
			{
				var fullLoadedPriceList = _pricingService.GetPricelistById(priceList.Id);
				priceList.Prices = fullLoadedPriceList.Prices.Where(x => x.ProductId == productId).ToList();
				result.Add(priceList.ToWebModel());
			}
			retVal = Ok(result.ToArray());
			return retVal;
		}


		// PUT: api/catalog/products/123/pricelists
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("api/catalog/products/{productId}/pricelists")]
		public IHttpActionResult UpdateProductPriceLists(string productId, webModel.Pricelist priceList)
		{
			var originalPriceList = _pricingService.GetPricelistById(priceList.Id);
			if (originalPriceList != null)
			{
				//Clean product prices in original pricelist
				var productPrices = originalPriceList.Prices.Where(x => x.ProductId == productId).ToArray();
				foreach(var productPrice in productPrices)
				{
					originalPriceList.Prices.Remove(productPrice);
				}
				//Add changed prices to original pricelist
				originalPriceList.Prices.AddRange(priceList.Prices.Select(x=>x.ToCoreModel()));
				_pricingService.UpdatePricelists(new coreModel.Pricelist[] { originalPriceList });
			}

			return StatusCode(HttpStatusCode.NoContent);
		}


	}
}
