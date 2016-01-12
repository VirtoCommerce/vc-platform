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
using VirtoCommerce.InventoryModule.Web.Security;
using VirtoCommerce.InventoryModule.Web.Binders;
using System.Web.Http.ModelBinding;

namespace VirtoCommerce.InventoryModule.Web.Controllers.Api
{
	[RoutePrefix("")]
	public class InventoryModuleController : ApiController
	{
		private readonly IInventoryService _inventoryService;
		private readonly ICommerceService _commerceService;
		public InventoryModuleController(IInventoryService inventoryService, ICommerceService commerceService)
		{
			_inventoryService = inventoryService;
			_commerceService = commerceService;
		}

        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>Get inventory of products for each fulfillment center.</remarks>
        /// <param name="ids">Products ids</param>
		[HttpGet]
		[ResponseType(typeof(webModel.InventoryInfo[]))]
		[Route("~/api/inventory/products")]
		public IHttpActionResult GetProductsInventories([ModelBinder(typeof(IdsStringArrayBinder))] string[] ids)
		{
			var result = new List<webModel.InventoryInfo>();
			var allFulfillments = _commerceService.GetAllFulfillmentCenters().ToArray();
			var inventories = _inventoryService.GetProductsInventoryInfos(ids).ToList();

			foreach (var productId in ids)
			{
				foreach (var fulfillment in allFulfillments)
				{
					var productInventory = inventories.FirstOrDefault(x => x.ProductId == productId && x.FulfillmentCenterId == fulfillment.Id)
					    ?? new coreModel.InventoryInfo { FulfillmentCenterId = fulfillment.Id, ProductId = productId };
				    
                    var webModelInventory = productInventory.ToWebModel();
					webModelInventory.FulfillmentCenter = fulfillment.ToWebModel();
					result.Add(webModelInventory);
				}
			}

			return Ok(result.ToArray());
		}

        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>Get inventories of product for each fulfillment center.</remarks>
        /// <param name="productId">Product id</param>
        [HttpGet]
		[ResponseType(typeof(webModel.InventoryInfo[]))]
		[Route("~/api/inventory/products/{productId}")]
		public IHttpActionResult GetProductInventories(string productId)
		{
			return GetProductsInventories(new [] { productId });
		}

        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>Upsert (add or update) given inventory of product.</remarks>
        /// <param name="inventory">Inventory to upsert</param>
		[HttpPut]
		[ResponseType(typeof(webModel.InventoryInfo))]
		[Route("~/api/inventory/products/{productId}")]
        [CheckPermission(Permission = InventoryPredefinedPermissions.Update)]
		public IHttpActionResult UpsertProductInventory(webModel.InventoryInfo inventory)
		{
			var result = _inventoryService.UpsertInventory( inventory.ToCoreModel() );

			return Ok(result);
		}


	}
}
