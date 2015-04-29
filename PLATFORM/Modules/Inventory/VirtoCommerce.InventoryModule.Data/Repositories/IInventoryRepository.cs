using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.InventoryModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.InventoryModule.Data.Repositories
{
	public interface IInventoryRepository : IRepository
	{
		IQueryable<Inventory> Inventories { get; }

		IEnumerable<Inventory> GetProductsInventories(string[] productIds);
	}
}
