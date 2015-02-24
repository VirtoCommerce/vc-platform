using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Inventory.Services;
using coreModel = VirtoCommerce.Domain.Inventory.Model;
using webModel = VirtoCommerce.InventoryModule.Web.Model;
using VirtoCommerce.InventoryModule.Web.Converters;

namespace VirtoCommerce.InventoryModule.Web.Controllers.Api
{
	[RoutePrefix("")]
	public class InventoryController : ApiController
	{
		private readonly IInventoryService _inventoryService;
		public InventoryController(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}

		// GET: api/catalog/products/{productId}/inventory
		[HttpGet]
		[ResponseType(typeof(webModel.InventoryInfo))]
		[Route("~/api/catalog/products/{productId}/inventory")]
		public IHttpActionResult GetProductInventory(string productId)
		{
			var retVal = _inventoryService.GetProductsInventoryInfos(new string[] { productId }).FirstOrDefault();
			if (retVal == null)
			{
				retVal = new coreModel.InventoryInfo
				{
					ProductId = productId,
					Status = coreModel.InventoryStatus.Enabled,
				};
			}
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/catalog/products/123/inventory
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("~/api/catalog/products/{productId}/inventory")]
		public IHttpActionResult UpdateProductInventory(webModel.InventoryInfo inventory)
		{
			_inventoryService.UpsertInventories(new coreModel.InventoryInfo[] { inventory.ToCoreModel() });

			return StatusCode(HttpStatusCode.NoContent);
		}


	}
}
