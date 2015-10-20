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
using VirtoCommerce.PricingModule.Web.Security;

namespace VirtoCommerce.PricingModule.Web.Controllers.Api
{
    [RoutePrefix("")]
    [CheckPermission(Permission = PricingPredefinedPermissions.Read)]
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

        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <param name="id">Pricelist assignment id</param>
        /// <response code="200"></response>
        /// <response code="404">Pricelist assignment not found.</response>
        [HttpGet]
        [ResponseType(typeof(webModel.PricelistAssignment))]
        [Route("api/pricing/assignments/{id}")]
        public IHttpActionResult GetPricelistAssignmentById(string id)
        {
            var assignment = _pricingService.GetPricelistAssignmentById(id);
            var result = assignment != null
                ? assignment.ToWebModel(null, _extensionManager.ConditionExpressionTree)
                : null;

            return result != null ? Ok(result) : (IHttpActionResult)NotFound();
        }

        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>Get array of all pricelist assignments for all catalogs.</remarks>
        /// <todo>Do we need return for all catalogs?</todo>
        [HttpGet]
        [ResponseType(typeof(webModel.PricelistAssignment[]))]
        [Route("api/pricing/assignments")]
        public IHttpActionResult GetPricelistAssignments()
        {
            var assignments = _pricingService.GetPriceListAssignments();
            var result  = assignments.Select(x => x.ToWebModel()).ToArray();
            return Ok(result);
        }

        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.</remarks>
        [HttpGet]
        [ResponseType(typeof(webModel.PricelistAssignment))]
        [Route("api/pricing/assignments/new")]
        public IHttpActionResult GetNewPricelistAssignments()
        {
            var result = new webModel.PricelistAssignment
            {
                Priority = 1,
                DynamicExpression = _extensionManager.ConditionExpressionTree
            };
            return Ok(result);
        }

        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        [HttpPost]
        [ResponseType(typeof(webModel.PricelistAssignment))]
        [Route("api/pricing/assignments")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Create)]
        public IHttpActionResult CreatePricelistAssignment(webModel.PricelistAssignment assignment)
        {
            var priceListAssignment = _pricingService.CreatePriceListAssignment(assignment.ToCoreModel());
            var result = priceListAssignment.ToWebModel(null, _extensionManager.ConditionExpressionTree);
            return Ok(result);
        }

        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <response code="204">Operation completed.</response>
        /// <todo>Return no any reason if can't update</todo>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/pricing/assignments")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Update)]
        public IHttpActionResult UpdatePriceListAssignment(webModel.PricelistAssignment assignment)
        {
            _pricingService.UpdatePricelistAssignments(new [] { assignment.ToCoreModel() });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>Delete pricelist assignment by given array of ids.</remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <response code="204">Operation completed.</response>
        /// <todo>Return no any reason if can't update</todo>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("api/pricing/assignments")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteAssignments([FromUri] string[] ids)
        {
            _pricingService.DeletePricelistsAssignments(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>Get an array of valid product prices for each currency.</remarks>
        /// <param name="productId">Product id</param>
        /// <response code="200"></response>
        /// <response code="404">Prices not found.</response>
        [HttpGet]
        [ResponseType(typeof(webModel.Price[]))]
        [Route("api/products/{productId}/prices")]
        public IHttpActionResult GetProductPrices(string productId)
        {
            var prices = _pricingService.EvaluateProductPrices(new coreModel.PriceEvaluationContext { ProductIds = new [] { productId } });
            var result = prices != null
                ? prices.GroupBy(x => x.Currency).Select(x => x.First().ToWebModel()).ToArray()
                : null;

            return result != null ? Ok(result) : (IHttpActionResult)NotFound(); 
        }

        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>Get all pricelists for all catalogs.</remarks>
        [HttpGet]
        [ResponseType(typeof(webModel.Pricelist[]))]
        [Route("api/pricing/pricelists")]
        public IHttpActionResult GetPriceLists()
        {
            var priceLists = _pricingService.GetPriceLists();
            var result = priceLists.Select(x => x.ToWebModel()).ToArray();
            return Ok(result);
        }

        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <param name="id">Pricelist id</param>
        /// <response code="200"></response>
        /// <response code="404">Pricelist not found.</response>
        [HttpGet]
        [ResponseType(typeof(webModel.Pricelist))]
        [Route("api/pricing/pricelists/{id}")]
        public IHttpActionResult GetPriceListById(string id)
        {
            webModel.Pricelist result = null;
            var pricelist = _pricingService.GetPricelistById(id);
            if (pricelist != null)
            {
                var productIds = pricelist.Prices.Select(x => x.ProductId).Distinct().ToArray();
                var products = _itemService.GetByIds(productIds, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
                var catalogs = _catalogService.GetCatalogsList().ToArray();
                result = pricelist.ToWebModel(products, catalogs, _extensionManager.ConditionExpressionTree);
            }
            return result != null ? Ok(result) : (IHttpActionResult)NotFound();
        }

        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>Get all pricelists for given product.</remarks>
        /// <param name="productId">Product id</param>
        /// <response code="200"></response>
        /// <response code="404">Pricelists not found.</response>
        /// <todo>I don't understand inherite algorithm. If product has two prices but variation has only one, then how (if need) variation does pick up inherite product prices</todo>
        [HttpGet]
        [ResponseType(typeof(webModel.Pricelist[]))]
        [Route("api/catalog/products/{productId}/pricelists")]
        public IHttpActionResult GetProductPriceLists(string productId)
        {
            var result = new List<webModel.Pricelist>();
            var priceLists = _pricingService.GetPriceLists();
            foreach (var priceList in priceLists)
            {
                var fullLoadedPriceList = _pricingService.GetPricelistById(priceList.Id);
                priceList.Prices = fullLoadedPriceList.Prices.Where(x => x.ProductId == productId).ToList();
				if(!priceList.Prices.Any())
				{
					//Price variation inheritance
					//Try to get price from main product for variation
					var product = _itemService.GetById(productId, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
					if(product.MainProductId != null)
					{
						priceList.Prices = fullLoadedPriceList.Prices.Where(x => x.ProductId == product.MainProductId).ToList();
						foreach(var price in priceList.Prices)
						{
							//For correct override price in possible update 
							price.Id = null;
							price.ProductId = productId;
						}
					}
				}
                result.Add(priceList.ToWebModel());
            }
            return result.Any() ? Ok(result.ToArray()) : (IHttpActionResult)NotFound();
        }


        /// <summary>
        /// Create pricelist
        /// </summary>
        [HttpPost]
        [ResponseType(typeof(webModel.Pricelist))]
        [Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Create)]
        public IHttpActionResult CreatePriceList(webModel.Pricelist priceList)
        {
            var pricelist = _pricingService.CreatePricelist(priceList.ToCoreModel());
            var result = pricelist.ToWebModel();
            return Ok(result);
        }

        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <response code="204">Operation completed.</response>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Update)]
        public IHttpActionResult UpdatePriceList(webModel.Pricelist priceList)
        {
            _pricingService.UpdatePricelists(new [] { priceList.ToCoreModel() });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Update prices  
        /// </summary>
        /// <remarks>Update prices of product for given pricelist.</remarks>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <response code="204">Operation completed.</response>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/catalog/products/{productId}/pricelists")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Update)]
        public IHttpActionResult UpdateProductPriceLists(string productId, webModel.Pricelist priceList)
        {
			var product = _itemService.GetById(productId, Domain.Catalog.Model.ItemResponseGroup.ItemInfo); //todo check uses
            var originalPriceList = _pricingService.GetPricelistById(priceList.Id);
            if (originalPriceList != null)
            {
                //Clean product prices in original pricelist
                var productPrices = originalPriceList.Prices.Where(x => x.ProductId == productId).ToArray();
                foreach (var productPrice in productPrices)
                {
                    originalPriceList.Prices.Remove(productPrice);
                }

                //Add changed prices to original pricelist
                originalPriceList.Prices.AddRange(priceList.ToCoreModel().Prices);
                _pricingService.UpdatePricelists(new [] { originalPriceList });
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Delete pricelists  
        /// </summary>
        /// <remarks>Delete pricelists by given array of pricelist ids.</remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <response code="204">Operation completed.</response>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PricingPredefinedPermissions.Delete)]
        public IHttpActionResult DeletePriceLists([FromUri] string[] ids)
        {
            _pricingService.DeletePricelists(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
