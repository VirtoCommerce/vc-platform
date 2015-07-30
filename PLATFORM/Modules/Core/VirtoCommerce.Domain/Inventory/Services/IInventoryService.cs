using System.Collections.Generic;
using VirtoCommerce.Domain.Inventory.Model;

namespace VirtoCommerce.Domain.Inventory.Services
{
	public interface IInventoryService
	{
	    IEnumerable<InventoryInfo> GetAllInventoryInfos();
        IEnumerable<InventoryInfo> GetProductsInventoryInfos(IEnumerable<string> productIds);
        InventoryInfo Create(InventoryInfo inventoryInfo);
		InventoryInfo UpsertInventory(InventoryInfo inventoryInfo);
		void UpsertInventories(IEnumerable<InventoryInfo> inventoryInfos);
	}
}
