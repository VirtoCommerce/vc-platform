using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.MarketingModule.Web.Security;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
    [RoutePrefix("api/marketing/promotions")]
    [CheckPermission(Permission = MarketingPredefinedPermissions.Read)]
    public class MarketingModulePromotionController : ApiController
    {
        private readonly IMarketingExtensionManager _marketingExtensionManager;
        private readonly IPromotionService _promotionService;
        private readonly IMarketingPromoEvaluator _promoEvaluator;

        public MarketingModulePromotionController(IPromotionService promotionService, IMarketingExtensionManager promotionManager, IMarketingPromoEvaluator promoEvaluator)
        {
            _marketingExtensionManager = promotionManager;
            _promotionService = promotionService;
            _promoEvaluator = promoEvaluator;
        }


        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <param name="context">Promotion evaluation context</param>
        [HttpPost]
        [ResponseType(typeof(webModel.PromotionReward[]))]
        [Route("evaluate")]
        public IHttpActionResult EvaluatePromotions(coreModel.PromotionEvaluationContext context)
        {
            var retVal = _promoEvaluator.EvaluatePromotion(context);
            return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
        }

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>Return a single promotion (dynamic or custom) object </remarks>
        /// <param name="id">promotion id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("{id}")]
        public IHttpActionResult GetPromotionById(string id)
        {
            var retVal = _promotionService.GetPromotionById(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel(_marketingExtensionManager.PromotionDynamicExpressionTree));
            }
            return NotFound();
        }

        /// <summary>
        /// Get new dynamic promotion object 
        /// </summary>
        /// <remarks>Return a new dynamic promotion object with populated dynamic expression tree</remarks>
        [HttpGet]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("new")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult GetNewDynamicPromotion()
        {
            var retVal = new webModel.Promotion
            {
                Type = typeof(DynamicPromotion).Name,
                DynamicExpression = _marketingExtensionManager.PromotionDynamicExpressionTree,
                IsActive = true
            };
            return Ok(retVal);
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        [HttpPost]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreatePromotion(webModel.Promotion promotion)
        {
            var retVal = _promotionService.CreatePromotion(promotion.ToCoreModel());
            return GetPromotionById(retVal.Id);
        }


        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <param name="promotion">>dynamic promotion object that needs to be updated in the marketing system</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdatePromotions(webModel.Promotion promotion)
        {
            _promotionService.UpdatePromotions(new coreModel.Promotion[] { promotion.ToCoreModel() });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///  Delete promotions objects
        /// </summary>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Delete)]
        public IHttpActionResult DeletePromotions([FromUri] string[] ids)
        {
            _promotionService.DeletePromotions(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
