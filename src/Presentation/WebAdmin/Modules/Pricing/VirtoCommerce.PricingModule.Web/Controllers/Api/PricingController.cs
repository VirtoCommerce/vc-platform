using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.PricingModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using webModel = VirtoCommerce.PricingModule.Web.Model;

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

		// GET: api/catalog/products/{productId}/prices
		[HttpGet]
		[ResponseType(typeof(webModel.Price[]))]
		[Route("api/products/{productId}/prices")]
		public IHttpActionResult GetProductPrices(string productId)
		{
			var retVal = _pricingService.EvaluateProductPrices(new coreModel.PriceEvaluationContext { ProductId = productId });
			if (retVal == null)
			{
				return NotFound();
			}
			return Ok(retVal.Select(x => x.ToWebModel()).ToArray());
		}

	
		// PUT: api/catalog/products/123/price
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("api/catalog/products/{productId}/price")]
		public IHttpActionResult UpdateProductPrices(webModel.Price[] prices)
		{
			foreach (var price in prices)
			{
				price.Id = null;
				_pricingService.CreatePrice(price.ToCoreModel());
			}

			return StatusCode(HttpStatusCode.NoContent);
		}


	}
}
