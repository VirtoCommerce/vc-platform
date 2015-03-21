using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Stores;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Data.Inventories;

namespace VirtoCommerce.InventoryModule.Data.Repositories
{
	public class FoundationInventoryRepositoryImpl : EFInventoryRepository, IFoundationInventoryRepository
	{
		public FoundationInventoryRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public FoundationInventoryRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		#region IFoundationInventoryRepository Members

	
		#endregion

		#region IFoundationInventoryRepository Members

		public IEnumerable<Inventory> GetProductsInventories(string[] productIds)
		{
			return base.Inventories.Where(x => productIds.Contains(x.Sku)).ToArray();
		}

		#endregion
	}

}
