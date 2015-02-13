using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Inventories.Repositories;
using foundation = VirtoCommerce.Foundation.Inventories.Model;
namespace VirtoCommerce.InventoryModule.Data.Repositories
{
	public interface IFoundationInventoryRepository : IInventoryRepository
	{
		IEnumerable<foundation.Inventory> GetProductsInventories(string[] productIds);
	}
}
