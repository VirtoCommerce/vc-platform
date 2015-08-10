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

        /// <summary>
        /// Get collection of pricelists for given catalog
        /// </summary>
        /// <param name="catalog">Catalog id (required)</param>
        /// <param name="currency">Currency code (optional)</param>
        /// <param name="tags">Array of tags (optional)</param>
		[HttpGet]
		[ClientCache(Duration = 60)]
		[ResponseType(typeof(string[]))]
		[Route("pricelists")]
		public IHttpActionResult GetPriceListStack(string catalog, string currency = null,	[FromUri] string[] tags = null)
		{
			var evaluationContext = new Domain.Pricing.Model.PriceEvaluationContext()
								{
									CatalogId = catalog,
									Currency = currency != null ? (CurrencyCodes?)Enum.Parse(typeof(CurrencyCodes), currency) : null,
									CertainDate = DateTime.UtcNow,
									Tags = tags
								};
			var strTags = tags != null ? String.Join(":", tags) : string.Empty;
			var cacheKey = CacheKey.Create("MP", "GetPriceListStack", catalog, currency, strTags);
			var retVal = _cacheManager.Get(cacheKey, () => _pricingService.EvaluatePriceLists(evaluationContext).Select(x=>x.Id).ToArray());
			return this.Ok(retVal);
		}

        /// <summary>
        /// Get prices collection for products and pricelists
        /// </summary>
        /// <param name="priceLists">Array of product ids (required)</param>
        /// <param name="products">Array of pricelists ids (required)</param>
		[HttpGet]
		[ResponseType(typeof(Price[]))]
		[ArrayInput(ParameterName = "priceLists")]
		[ArrayInput(ParameterName = "products")]
		[Route("prices")]
		public IHttpActionResult GetProductPrices([FromUri] string[] priceLists, [FromUri] string[] products)
		{
			var retVal = new List<Price>();

			var evalContext = new Domain.Pricing.Model.PriceEvaluationContext()
							{
								ProductIds = products,
								PricelistIds = priceLists
							};
			var strProducts = priceLists != null ? String.Join(":", products) : string.Empty;
			var strPriceLists = products != null ? String.Join(":", priceLists) : string.Empty;
			var cacheKey = CacheKey.Create("MP", "GetProductPrices", strProducts, strPriceLists);
			var prices = _cacheManager.Get(cacheKey, () => _pricingService.EvaluateProductPrices(evalContext));
			retVal.AddRange(prices.Select(x => x.ToWebModel()));

			return Ok(retVal);
		}


	}
}