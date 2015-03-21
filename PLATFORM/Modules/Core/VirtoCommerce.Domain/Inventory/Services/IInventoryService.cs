using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Inventory.Model;

namespace VirtoCommerce.Domain.Inventory.Services
{
	public interface IInventoryService
	{
		IEnumerable<InventoryInfo> GetProductsInventoryInfos(IEnumerable<string> productIds);
		InventoryInfo UpsertInventory(InventoryInfo inventoryInfo);
		void UpsertInventories(IEnumerable<InventoryInfo> inventoryInfos);
	}
}
