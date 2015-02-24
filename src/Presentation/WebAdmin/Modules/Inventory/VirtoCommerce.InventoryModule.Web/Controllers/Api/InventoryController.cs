using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Inventory.Services;
using coreModel = VirtoCommerce.Domain.Inventory.Model;
using webModel = VirtoCommerce.InventoryModule.Web.Model;
using VirtoCommerce.InventoryModule.Web.Converters;
using VirtoCommerce.Domain.Fulfillment.Services;

namespace VirtoCommerce.InventoryModule.Web.Controllers.Api
{
	[RoutePrefix("")]
	public class InventoryController : ApiController
	{
		private readonly IInventoryService _inventoryService;
		private readonly IFulfillmentService _fulfillmentService;
		public InventoryController(IInventoryService inventoryService, IFulfillmentService fulfillmentService)
		{
			_inventoryService = inventoryService;
			_fulfillmentService = fulfillmentService;
		}

		// GET: api/catalog/products/{productId}/inventory
		[HttpGet]
		[ResponseType(typeof(webModel.InventoryInfo[]))]
		[Route("~/api/catalog/products/{productId}/inventory")]
		public IHttpActionResult GetProductInventories(string productId)
		{
			var allFulfillments = _fulfillmentService.GetAllFulfillmentCenters();
			var retVal = _inventoryService.GetProductsInventoryInfos(new string[] { productId }).ToList();
			foreach (var fulfillment in allFulfillments)
			{
				var inventoryWithFulfillment = retVal.FirstOrDefault(x => x.FulfillmentCenterId == fulfillment.Id);
				if (inventoryWithFulfillment == null)
				{
					inventoryWithFulfillment = new coreModel.InventoryInfo
					{
						FulfillmentCenterId = fulfillment.Id,
						ProductId = productId
					};
					retVal.Add(inventoryWithFulfillment);
				}
			}
		
			return Ok(retVal.Select(x=>x.ToWebModel()).ToArray());
		}

		// PUT: api/catalog/products/123/inventory
		[HttpPut]
		[ResponseType(typeof(webModel.InventoryInfo))]
		[Route("~/api/catalog/products/{productId}/inventory")]
		public IHttpActionResult UpsertProductInventory(webModel.InventoryInfo inventory)
		{
			var retVal = _inventoryService.UpsertInventory( inventory.ToCoreModel() );

			return Ok(retVal);
		}


	}
}
