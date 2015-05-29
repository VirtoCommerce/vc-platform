using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CustomerModule.Web.Binders;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing/promotions")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
    public class MarketingModulePromotionController : ApiController
    {
		private readonly IMarketingExtensionManager _marketingExtensionManager;
		private readonly IPromotionService _promotionService;

		public MarketingModulePromotionController(IPromotionService promotionService, 	IMarketingExtensionManager promotionManager)
		{
			_marketingExtensionManager = promotionManager;
			_promotionService = promotionService;
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
				return Ok(retVal.ToWebModel(_marketingExtensionManager.PromotionDynamicExpressionTree)); 
			}
			return NotFound();
		}

        // GET: api/marketing/promotions/new
        [HttpGet]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("new")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
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

		// POST: api/marketing/promotions
		[HttpPost]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreatePromotion(webModel.Promotion promotion)
		{
			var retVal = _promotionService.CreatePromotion(promotion.ToCoreModel());
			return GetPromotionById(retVal.Id);
		}


		// PUT: api/marketing/promotions
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdatePromotions(webModel.Promotion promotion)
		{
			_promotionService.UpdatePromotions(new coreModel.Promotion[] { promotion.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/promotions?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeletePromotions([FromUri] string[] ids)
		{
			_promotionService.DeletePromotions(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
    }
}
