using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Web.Converters;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing")]
    public class MarketingController : ApiController
    {
		private readonly IMarketingPromoEvaluator _promotionEvaluator;
		public MarketingController(IMarketingPromoEvaluator promotionEvaluator)
		{
			_promotionEvaluator = promotionEvaluator;
		}

		// GET: api/marketing/promotions/evaluate
		[HttpPost]
		[ResponseType(typeof(webModel.PromotionReward[]))]
		[Route("promotions/evaluate")]
		public IHttpActionResult Evaluate(PromotionEvaluationContext context)
		{
			var retVal = _promotionEvaluator.EvaluatePromotion(context);
			return Ok(retVal.Rewards.Select(x=>x.ToWebModel()).ToArray());
		}

		// GET: api/marketing/processevent
		[HttpPost]
		[ResponseType(typeof(webModel.PromotionReward[]))]
		[Route("processevent")]
		public IHttpActionResult ProcessEvent(webModel.MarketingEvent marketingEvent)
		{
			var retVal = _promotionEvaluator.ProcessEvent(marketingEvent.ToCoreModel());
			return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
		}

	
    }
}
