using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.PricingModule.Web.Converters;
using webModel = VirtoCommerce.PricingModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Common;

namespace VirtoCommerce.PricingModule.Web.Controllers.Api
{
	[RoutePrefix("")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
	public class PricingModuleController : ApiController
	{
		private readonly IPricingService _pricingService;
		private readonly IItemService _itemService;
		private readonly ICatalogService _catalogService;
		private readonly IPricingExtensionManager _extensionManager;
		public PricingModuleController(IPricingService pricingService, IItemService itemService, ICatalogService catalogService, IPricingExtensionManager extensionManager)
		{
			_extensionManager = extensionManager;
			_pricingService = pricingService;
			_itemService = itemService;
			_catalogService = catalogService;
		}

		// GET: api/pricing/assignments/id
		[HttpGet]
		[ResponseType(typeof(webModel.PricelistAssignment))]
		[Route("api/pricing/assignments/{id}")]
		public IHttpActionResult GetPricelistAssignmentById(string id)
		{
			var assignment = _pricingService.GetPricelistAssignmentById(id);
			if(assignment == null)
			{
				return NotFound();
			}
			return Ok(assignment.ToWebModel(null, _extensionManager.ConditionExpressionTree as ConditionExpressionTree));
		}

		// GET: api/pricing/assignments
		[HttpGet]
		[ResponseType(typeof(webModel.PricelistAssignment[]))]
		[Route("api/pricing/assignments")]
		public IHttpActionResult GetPricelistAssignments()
		{
			var assignments = _pricingService.GetPriceListAssignments();
			var retVal = Ok(assignments.Select(x => x.ToWebModel()).ToArray());
			return retVal;
		}

		// POST: api/pricing/assignments
		[HttpPost]
        [ResponseType(typeof(webModel.PricelistAssignment))]
		[Route("api/pricing/assignments")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreatePriceList(webModel.PricelistAssignment assignment)
		{
			var retVal = _pricingService.CreatePriceListAssignment(assignment.ToCoreModel());
			return Ok(retVal.ToWebModel(null, _extensionManager.ConditionExpressionTree));
		}

		// PUT: api/pricing/assignments
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("api/pricing/assignments")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdatePriceListAssignment(webModel.PricelistAssignment assignment)
		{
			_pricingService.UpdatePricelistAssignments(new coreModel.PricelistAssignment[] { assignment.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: /api/pricing/assignments?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("api/pricing/assignments")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteAssignments([FromUri] string[] ids)
		{
			_pricingService.DeletePricelistsAssignments(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}

		// GET: api/products/{productId}/prices
		[HttpGet]
		[ResponseType(typeof(webModel.Price[]))]
		[Route("api/products/{productId}/prices")]
		public IHttpActionResult GetProductPrices(string productId)
		{
			IHttpActionResult retVal = NotFound();
			var prices = _pricingService.EvaluateProductPrices(new coreModel.PriceEvaluationContext { ProductIds = new string[] { productId } });
			if (prices != null)
			{
				retVal = Ok(prices.GroupBy(x=>x.Currency).Select(x => x.First().ToWebModel()).ToArray());
			}
			return retVal;
		}

		// GET: api/pricing/pricelists
		[HttpGet]
		[ResponseType(typeof(webModel.Pricelist[]))]
		[Route("api/pricing/pricelists")]
		public IHttpActionResult GetPriceLists()
		{
			var priceLists = _pricingService.GetPriceLists();
			var retVal = Ok(priceLists.Select(x => x.ToWebModel()).ToArray());
			return retVal;
		}

		// GET: api/pricing/pricelists/id
		[HttpGet]
		[ResponseType(typeof(webModel.Pricelist))]
		[Route("api/pricing/pricelists/{id}")]
		public IHttpActionResult GetPriceListById(string id)
		{
			IHttpActionResult retVal = NotFound();
			var result = _pricingService.GetPricelistById(id);
			var productIds = result.Prices.Select(x => x.ProductId).Distinct().ToArray();
			var products = _itemService.GetByIds(productIds, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
			var catalogs = _catalogService.GetCatalogsList().ToArray();
			if (result != null)
			{
				retVal = Ok(result.ToWebModel(products, catalogs));
			}
			return retVal;
		}

		// GET: api/catalog/products/{productId}/pricelists
		[HttpGet]
		[ResponseType(typeof(webModel.Pricelist[]))]
		[Route("api/catalog/products/{productId}/pricelists")]
		public IHttpActionResult GetProductPriceLists(string productId)
		{
			var result = new List<webModel.Pricelist>();
			IHttpActionResult retVal = NotFound();
			var priceLists = _pricingService.GetPriceLists();
			foreach (var priceList in priceLists)
			{
				var fullLoadedPriceList = _pricingService.GetPricelistById(priceList.Id);
				priceList.Prices = fullLoadedPriceList.Prices.Where(x => x.ProductId == productId).ToList();
				result.Add(priceList.ToWebModel());
			}
			retVal = Ok(result.ToArray());
			return retVal;
		}


		// POST: api/pricing/pricelists
		[HttpPost]
		[ResponseType(typeof(webModel.Pricelist))]
		[Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreatePriceList(webModel.Pricelist priceList)
		{
			var retVal = _pricingService.CreatePricelist(priceList.ToCoreModel());
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/pricing/pricelists
		[HttpPut]
        [ResponseType(typeof(void))]
		[Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdatePriceList(webModel.Pricelist priceList)
		{
			_pricingService.UpdatePricelists(new coreModel.Pricelist[] {  priceList.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// PUT: api/catalog/products/123/pricelists
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("api/catalog/products/{productId}/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdateProductPriceLists(string productId, webModel.Pricelist priceList)
		{
			var originalPriceList = _pricingService.GetPricelistById(priceList.Id);
			if (originalPriceList != null)
			{
				//Clean product prices in original pricelist
				var productPrices = originalPriceList.Prices.Where(x => x.ProductId == productId).ToArray();
				foreach(var productPrice in productPrices)
				{
					originalPriceList.Prices.Remove(productPrice);
				}

				//Add changed prices to original pricelist
				originalPriceList.Prices.AddRange(priceList.ToCoreModel().Prices);
				_pricingService.UpdatePricelists(new coreModel.Pricelist[] { originalPriceList });
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		
		// DELETE: /api/pricing/pricelists?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeletePriceLists([FromUri] string[] ids)
		{
			_pricingService.DeletePricelists(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
