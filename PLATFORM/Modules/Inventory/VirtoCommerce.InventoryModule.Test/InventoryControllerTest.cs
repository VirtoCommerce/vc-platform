using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.InventoryModule.Data.Repositories;
using VirtoCommerce.InventoryModule.Data.Services;
using VirtoCommerce.InventoryModule.Web.Controllers.Api;
using VirtoCommerce.InventoryModule.Web.Model;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.InventoryModule.Test
{
	[TestClass]
	public class InventoryControllerTest
	{
		[TestMethod]
		public void ChangeProductInventory()
		{
			//Get product inventory
			var controller = GetController();
			var result = controller.GetProductInventories("v-b005gs3cfg") as OkNegotiatedContentResult<InventoryInfo[]>;
			var inventory = result.Content.FirstOrDefault();
			inventory.InStockQuantity += 20;
			controller.UpsertProductInventory(inventory);
		
		}

		private static InventoryModuleController GetController()
		{
			
			var controller = new InventoryModuleController(null, null);
			return controller;
		}
	}
}
