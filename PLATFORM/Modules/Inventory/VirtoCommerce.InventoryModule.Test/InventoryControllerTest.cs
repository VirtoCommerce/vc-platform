using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CoreModule.Web.Repositories;
using VirtoCommerce.CoreModule.Web.Services;
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
			var result = controller.GetProductInventories("v-b005gs3cfg") as OkNegotiatedContentResult<InventoryInfo[]>;
			var inventory = result.Content.FirstOrDefault();
			inventory.InStockQuantity += 20;
			controller.UpsertProductInventory(inventory);
		
		}

		private static InventoryController GetController()
		{
			Func<IFoundationInventoryRepository> repositoryFactory = () =>
			{
				return new FoundationInventoryRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};

			Func<IFoundationFulfillmentRepository> fulfillRepositoryFactory = () =>
			{
				return new FoundationFulfillmentRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			
			var service = new InventoryServiceImpl(repositoryFactory);
			var fulfillmentService = new FulfillmentServiceImpl(fulfillRepositoryFactory);
			var controller = new InventoryController(service, fulfillmentService);
			return controller;
		}
	}
}
