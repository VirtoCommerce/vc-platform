using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Inventory.Model;
using webModel = VirtoCommerce.InventoryModule.Web.Model;
using VirtoCommerce.InventoryModule.Web.Converters;
using VirtoCommerce.Domain.Commerce.Services;
using System.Collections.Generic;

namespace VirtoCommerce.InventoryModule.Web.Controllers.Api
{
	[RoutePrefix("")]
	public class InventoryController : ApiController
	{
		private readonly IInventoryService _inventoryService;
		private readonly ICommerceService _commerceService;
		public InventoryController(IInventoryService inventoryService, ICommerceService commerceService)
		{
			_inventoryService = inventoryService;
			_commerceService = commerceService;
		}

		// GET: api/inventory/products?ids=212&ids=333
		[HttpGet]
		[ResponseType(typeof(webModel.InventoryInfo[]))]
		[Route("~/api/inventory/products")]
		public IHttpActionResult GetProductsInventories([FromUri] string[] ids)
		{
			var retVal = new List<webModel.InventoryInfo>();
			var allFulfillments = _commerceService.GetAllFulfillmentCenters();
			var inventories = _inventoryService.GetProductsInventoryInfos(ids).ToList();

			foreach (var productId in ids)
			{
				foreach (var fulfillment in allFulfillments)
				{
					var productInventory = inventories.FirstOrDefault(x => x.ProductId == productId && x.FulfillmentCenterId == fulfillment.Id);
					if (productInventory == null)
					{
						productInventory = new coreModel.InventoryInfo
						{
							FulfillmentCenterId = fulfillment.Id,
							ProductId = productId
						};
					}
					var webModelInventory = productInventory.ToWebModel();
					webModelInventory.FulfillmentCenter = fulfillment.ToWebModel();
					retVal.Add(webModelInventory);
				}
			}

			return Ok(retVal.ToArray());
		}

		// GET: api/inventory/products/{productId}
		[HttpGet]
		[ResponseType(typeof(webModel.InventoryInfo[]))]
		[Route("~/api/inventory/products/{productId}")]
		public IHttpActionResult GetProductInventories(string productId)
		{
			return GetProductsInventories(new string[] { productId });
		}

		// PUT: api/inventory/products/123/inventory
		[HttpPut]
		[ResponseType(typeof(webModel.InventoryInfo))]
		[Route("~/api/inventory/products/{productId}")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpsertProductInventory(webModel.InventoryInfo inventory)
		{
			var retVal = _inventoryService.UpsertInventory( inventory.ToCoreModel() );

			return Ok(retVal);
		}


	}
}
