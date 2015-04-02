using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using ExpressionSerialization;
using VirtoCommerce.CustomerModule.Web.Binders;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Domain.Marketing.Services;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.MarketingModule.Web.Converters;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing")]
    public class MarketingManagmentController : ApiController
    {
		private readonly ICustomPromotionManager _promotionManager;
		private readonly IMarketingService _marketingService;
		private readonly IMarketingSearchService _marketingSearchService;
		public MarketingManagmentController(IMarketingService marketingService, IMarketingSearchService marketingSearchService,
											ICustomPromotionManager promotionManager)
		{
			_promotionManager = promotionManager;
			_marketingService = marketingService;
			_marketingSearchService = marketingSearchService;
		
		}

		// GET: api/marketing/promotions?q=ddd&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(coreModel.SearchResult))]
		[Route("promotions")]
		public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] coreModel.SearchCriteria criteria)
		{
			var retVal = new webModel.SearchResult();
			var coreResult = _marketingSearchService.SearchPromotions(criteria);
			retVal.Promotions = coreResult.Promotions.Select(x => x.ToWebModel()).ToList();
			retVal.TotalCount = coreResult.TotalCount;
			return Ok(retVal);
		}

		// GET: api/marketing/promotions/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("promotions/{id}")]
		public IHttpActionResult GetPromotionById(string id)
		{
			var retVal = _marketingService.GetPromotionById(id);
			if(retVal != null)
			{
				return Ok(retVal.ToWebModel(_promotionManager.DynamicExpression)); 
			}
			return NotFound();
		}

		// GET: api/marketing/promotions/new
		[HttpPost]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("promotions/new")]
		public IHttpActionResult GetNewDynamicPromotion()
		{
			var retVal = new webModel.Promotion
			{
				Type = webModel.PromotionType.Dynamic,
				DynamicExpression = _promotionManager.DynamicExpression
			};
			return Ok(retVal);
		}

		// POST: api/marketing/promotions
		[HttpPost]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("promotions")]
		public IHttpActionResult CreatePromotion(webModel.Promotion promotion)
		{
			var retVal = _marketingService.CreatePromotion(promotion.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}


		// PUT: api/marketing/promotions
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("promotions")]
		public IHttpActionResult UpdatePromotions(webModel.Promotion promotion)
		{
			_marketingService.UpdatePromotions(new coreModel.Promotion[] { promotion.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/promotions?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("promotions")]
		public IHttpActionResult DeletePromotions([FromUri] string[] ids)
		{
			return StatusCode(HttpStatusCode.NoContent);
		}
    }
}
