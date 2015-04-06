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
using VirtoCommerce.Domain.Marketing.Services;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.MarketingModule.Web.Converters;
using Newtonsoft.Json;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions;
using VirtoCommerce.MarketingModule.Data;

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
