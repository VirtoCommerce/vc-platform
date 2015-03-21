using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Inventories.Repositories
{
	public interface IInventoryRepository : IRepository
	{
		IQueryable<Inventory> Inventories { get; }
	}
}
