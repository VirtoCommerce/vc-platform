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
		InventoryInfo GetProductInventoryInfo(string productId);
		void UpsertInventory(InventoryInfo inventoryInfo);
	}
}
