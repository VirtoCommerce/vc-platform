using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.InventoryModule.Data.Repositories;
using VirtoCommerce.InventoryModule.Data.Services;
using VirtoCommerce.InventoryModule.Web.Controllers.Api;
using VirtoCommerce.InventoryModule.Web.Model;

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
			var result = controller.GetProductInventory("v-b005gs3cfg") as OkNegotiatedContentResult<InventoryInfo>;
			InventoryInfo inventory = null;
			if(result != null)
			{
				inventory = result.Content;
			}
			if(inventory == null)
			{
				inventory = new InventoryInfo
				{
					FulfillmentCenterId = "default",
					ProductId = "v-b005gs3cfg",
					Status = Domain.Inventory.Model.InventoryStatus.Enabled
				};
			}
			inventory.InStockQuantity += 20;
			controller.UpdateProductInventory(inventory);
		
		}

	
		private static InventoryController GetController()
		{
			Func<IFoundationInventoryRepository> repositoryFactory = () =>
			{
				return new FoundationInventoryRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			
			var service = new InventoryServiceImpl(repositoryFactory);
			var controller = new InventoryController(service);
			return controller;
		}
	}
}
