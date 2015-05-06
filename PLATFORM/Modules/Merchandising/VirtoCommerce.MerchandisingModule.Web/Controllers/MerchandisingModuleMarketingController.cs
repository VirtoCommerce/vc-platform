using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using System.Security.Cryptography;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp")]
	public class MerchandisingModuleMarketingController : ApiController
    {
		private readonly IMarketingPromoEvaluator _promotionEvaluator;
		private readonly IMarketingDynamicContentEvaluator _contentEvaluator;
		private readonly CacheManager _cacheManager;

		public MerchandisingModuleMarketingController(IMarketingPromoEvaluator promotionEvaluator, IMarketingDynamicContentEvaluator contentEvaluator, CacheManager cacheManager) 
        {
			_promotionEvaluator = promotionEvaluator;
			_contentEvaluator = contentEvaluator;
			_cacheManager = cacheManager;
        }

		// POST: api/mp/promotions/evaluate
		[HttpPost]
		[ResponseType(typeof(webModel.PromotionReward[]))]
		[Route("promotions/evaluate")]
		[ClientCache(Duration = 1)]
		public IHttpActionResult EvaluatePromotions(coreModel.PromotionEvaluationContext context)
		{
            // DOESN'T WORK
			//var cacheKey = CacheKey.Create("MarketingController.EvaluatePromotions", context.GetHash<MD5CryptoServiceProvider>());
			//var retVal = _cacheManager.Get(cacheKey, () => _promotionEvaluator.EvaluatePromotion(context));
		    var retVal = _promotionEvaluator.EvaluatePromotion(context);
			return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
		}

		// POST: api/mp/promotions/processevent
		[HttpPost]
		[ResponseType(typeof(webModel.PromotionReward[]))]
		[Route("promotions/processevent")]
		public IHttpActionResult ProcessMarketingEvent(coreModel.IMarketingEvent marketingEvent)
		{
			var retVal = _promotionEvaluator.ProcessEvent(marketingEvent);
			return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
		}

		// POST: api/mp/contentitems/evaluate
		[HttpPost]
		[ResponseType(typeof(webModel.DynamicContentItem[]))]
		[Route("contentitems/evaluate")]
		public IHttpActionResult EvaluateDynamicContent(coreModel.DynamicContent.DynamicContentEvaluationContext context)
		{
			var cacheKey = CacheKey.Create("MarketingController.EvaluateDynamicContent", context.GetHash<MD5CryptoServiceProvider>());
			var retVal = _cacheManager.Get(cacheKey, () => _contentEvaluator.EvaluateItems(context));
			return Ok(retVal.Select(x => x.ToWebModel()).ToArray());
		}


    }
}
