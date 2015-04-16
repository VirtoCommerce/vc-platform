using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CustomerModule.Web.Binders;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Expressions.Promotion;
using VirtoCommerce.MarketingModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing/promotions")]
    public class PromotionController : ApiController
    {
		private readonly IMarketingExtensionManager _promotionManager;
		private readonly IPromotionService _promotionService;
		private readonly IMarketingPromoEvaluator _promotionEvaluator;

		public PromotionController(IPromotionService promotionService, 	IMarketingExtensionManager promotionManager, IMarketingPromoEvaluator promotionEvaluator)
		{
			_promotionManager = promotionManager;
			_promotionService = promotionService;
			_promotionEvaluator = promotionEvaluator;
		}

		// POST: api/marketing/promotions/evaluate
		[HttpPost]
		[ResponseType(typeof(webModel.PromotionReward[]))]
		[Route("evaluate")]
		public IHttpActionResult Evaluate(PromotionEvaluationContext context)
		{
			var retVal = _promotionEvaluator.EvaluatePromotion(context);
			return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
		}

		// POST: api/marketing/promotions/processevent
		[HttpPost]
		[ResponseType(typeof(webModel.PromotionReward[]))]
		[Route("processevent")]
		public IHttpActionResult ProcessEvent(webModel.MarketingEvent marketingEvent)
		{
			var retVal = _promotionEvaluator.ProcessEvent(marketingEvent.ToCoreModel());
			return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
		}

		// GET: api/marketing/promotions/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("{id}")]
		public IHttpActionResult GetPromotionById(string id)
		{
			var retVal = _promotionService.GetPromotionById(id);
			if(retVal != null)
			{
				return Ok(retVal.ToWebModel(_promotionManager.PromotionDynamicExpressionTree as PromoDynamicExpressionTree)); 
			}
			return NotFound();
		}

        // GET: api/marketing/promotions/new
        [HttpGet]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("new")]
        public IHttpActionResult GetNewDynamicPromotion()
        {
            var retVal = new webModel.Promotion
            {
				Type = typeof(DynamicPromotion).Name,
				DynamicExpression = _promotionManager.PromotionDynamicExpressionTree as PromoDynamicExpressionTree,
				IsActive = true
            };
            return Ok(retVal);
        }

		// POST: api/marketing/promotions
		[HttpPost]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("")]
		public IHttpActionResult CreatePromotion(webModel.Promotion promotion)
		{
			var retVal = _promotionService.CreatePromotion(promotion.ToCoreModel());
			return GetPromotionById(retVal.Id);
		}


		// PUT: api/marketing/promotions
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult UpdatePromotions(webModel.Promotion promotion)
		{
			_promotionService.UpdatePromotions(new coreModel.Promotion[] { promotion.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/promotions?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult DeletePromotions([FromUri] string[] ids)
		{
			_promotionService.DeletePromotions(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
    }
}
