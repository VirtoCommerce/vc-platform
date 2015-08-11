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

        /// <summary>
        /// Return PricelistAssignment by given Id
        /// </summary>
        /// <remarks>Return PricelistAssignment by given Id</remarks>
        /// <param name="id">PricelistAssignment Id</param>
        /// <response code="200">PricelistAssignment returned OK</response>
        [HttpGet]
        [ResponseType(typeof(webModel.PricelistAssignment))]
        [Route("api/pricing/assignments/{id}")]
        public IHttpActionResult GetPricelistAssignmentById(string id)
        {
            var assignment = _pricingService.GetPricelistAssignmentById(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment.ToWebModel(null, _extensionManager.ConditionExpressionTree));
        }

        /// <summary>
        /// Return list of all PricelistAssignments
        /// </summary>
        /// <remarks>Return list of all PricelistAssignments</remarks>
        /// <response code="200">PricelistAssignments returned OK</response>
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
        /// Return a new PricelistAssignment
        /// </summary>
        /// <remarks>Return a new PricelistAssignment object. It doesn't create or save the PricelistAssignment object</remarks>
        /// <response code="200">PricelistAssignment returned OK</response>
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
        /// Create PricelistAssignment
        /// </summary>
        /// <remarks>Create PricelistAssignment</remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <response code="200">PricelistAssignment returned OK</response>
        /// <todo>Rename the method</todo>
        [HttpPost]
        [ResponseType(typeof(webModel.PricelistAssignment))]
        [Route("api/pricing/assignments")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult CreatePriceList(webModel.PricelistAssignment assignment)
        {
            var priceListAssignment = _pricingService.CreatePriceListAssignment(assignment.ToCoreModel());
            var result = priceListAssignment.ToWebModel(null, _extensionManager.ConditionExpressionTree);
            return Ok(result);
        }

        /// <summary>
        /// Update PricelistAssignment
        /// </summary>
        /// <remarks>Update PricelistAssignment</remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <response code="204">No content</response>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/pricing/assignments")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult UpdatePriceListAssignment(webModel.PricelistAssignment assignment)
        {
            _pricingService.UpdatePricelistAssignments(new [] { assignment.ToCoreModel() });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete PricelistAssignments
        /// </summary>
        /// <remarks>Delete PricelistAssignments by given Ids</remarks>
        /// <param name="ids">List of PricelistAssignment Ids to delete</param>
        /// <response code="204">No content</response>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("api/pricing/assignments")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult DeleteAssignments([FromUri] string[] ids)
        {
            _pricingService.DeletePricelistsAssignments(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Return list of Product prices
        /// </summary>
        /// <remarks>Return list of valid Product prices for each currency</remarks>
        /// <param name="productId">Product Id</param>
        /// <response code="200">Product prices returned OK</response>
        [HttpGet]
        [ResponseType(typeof(webModel.Price[]))]
        [Route("api/products/{productId}/prices")]
        public IHttpActionResult GetProductPrices(string productId)
        {
            var prices = _pricingService.EvaluateProductPrices(new coreModel.PriceEvaluationContext { ProductIds = new [] { productId } });
            var result = prices != null
                ? prices.GroupBy(x => x.Currency).Select(x => x.First().ToWebModel()).ToArray()
                : null;

            return result != null ? Ok(result) : (IHttpActionResult)NotFound(); //todo desctibe NotFound
        }

        /// <summary>
        /// Return all Pricelists
        /// </summary>
        /// <remarks>Return all Pricelists</remarks>
        /// <response code="200">Pricelists returned OK</response>
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
        /// Return Pricelist
        /// </summary>
        /// <remarks>Return pricelist by given Id</remarks>
        /// <param name="id">Pricelist Id</param>
        /// <response code="200">Pricelist returned OK</response>
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
            return result != null ? Ok(result) : (IHttpActionResult)NotFound(); //todo desctibe NotFound
        }

        /// <summary>
        /// Return Pricelists for a product
        /// </summary>
        /// <remarks>Return Pricelists for a product by given product Id</remarks>
        /// <param name="productId">Product Id</param>
        /// <response code="200">Pricelists returned OK</response>
        /// <todo>I don't understand inherite algorithm. If product has two prices (different quantity of batch) but variation has only one, then how (if need) variation does pick up inherite product prices</todo>
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
        /// Create Pricelist
        /// </summary>
        /// <remarks>Create Pricelist</remarks>
        /// <param name="priceList">Pricelist</param>
        /// <response code="200">Pricelist returned OK</response>
        [HttpPost]
        [ResponseType(typeof(webModel.Pricelist))]
        [Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult CreatePriceList(webModel.Pricelist priceList)
        {
            var pricelist = _pricingService.CreatePricelist(priceList.ToCoreModel());
            var result = pricelist.ToWebModel();
            return Ok(result);
        }

        /// <summary>
        /// Update Pricelist
        /// </summary>
        /// <remarks>Update Pricelist</remarks>
        /// <param name="priceList">Pricelist</param>
        /// <response code="200">Pricelist returned OK</response>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/pricing/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult UpdatePriceList(webModel.Pricelist priceList)
        {
            _pricingService.UpdatePricelists(new [] { priceList.ToCoreModel() });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Update prices of Product for a Pricelist  
        /// </summary>
        /// <remarks>Update prices of given Product for a given Pricelist</remarks>
        /// <param name="productId">Product Id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <response code="204">No content</response>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/catalog/products/{productId}/pricelists")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
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
        /// Delete Pricelists  
        /// </summary>
        /// <remarks>Delete Pricelists by given Pricelist</remarks>
        /// <param name="ids">Pricelist Ids to delete</param>
        /// <response code="204">No content</response>
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
