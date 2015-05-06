using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp")]
	public class MerchandisingModulePriceController : ApiController
	{
		private readonly IPricingService _pricingService;
		private readonly CacheManager _cacheManager;

	
		public MerchandisingModulePriceController(IPricingService pricingService, CacheManager cacheManager)
		{
			_pricingService = pricingService;
			_cacheManager = cacheManager;
		}

		[HttpGet]
		[ClientCache(Duration = 60)]
		[ResponseType(typeof(string[]))]
		[Route("pricelists")]
		public IHttpActionResult GetPriceListStack(string catalog, string currency,	[FromUri] string[] tags)
		{
			var cacheKey = CacheKey.Create("PriceController.GetPriceListStack", catalog, currency);
			var retVal = _cacheManager.Get(cacheKey, () => _pricingService.GetPriceLists().Select(x=>x.Id).ToArray());
			return this.Ok(retVal);
		}

		[HttpGet]
		[ResponseType(typeof(Price[]))]
		[ArrayInput(ParameterName = "products")]
		[Route("prices")]
		public IHttpActionResult GetProductPrices([FromUri] string[] products)
		{
			var retVal = new List<Price>();
			foreach (var product in products)
			{
				var evalContext = new Domain.Pricing.Model.PriceEvaluationContext()
								{
									ProductId = product
								};
				var cacheKey = CacheKey.Create("PriceController.GetProductPrices", product);
				var prices = _cacheManager.Get(cacheKey, () => _pricingService.EvaluateProductPrices(evalContext));
				retVal.AddRange(prices.Select(x => x.ToWebModel()));
	
			}
			return Ok(retVal);
		}


	}
}