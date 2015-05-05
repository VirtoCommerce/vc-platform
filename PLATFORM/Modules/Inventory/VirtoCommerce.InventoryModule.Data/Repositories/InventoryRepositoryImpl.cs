using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.InventoryModule.Data.Model;

namespace VirtoCommerce.InventoryModule.Data.Repositories
{
	public class InventoryRepositoryImpl : EFRepositoryBase, IInventoryRepository
	{
		public InventoryRepositoryImpl()
		{
		}

		public InventoryRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public InventoryRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<Inventory>(modelBuilder, toTable: "Inventory");

			base.OnModelCreating(modelBuilder);
		}


		#region IFoundationInventoryRepository Members

		public IQueryable<Inventory> Inventories
		{
			get { return GetAsQueryable<Inventory>(); }
		}

		public IEnumerable<Inventory> GetProductsInventories(string[] productIds)
		{
			return Inventories.Where(x => productIds.Contains(x.Sku)).ToArray();
		}

		#endregion
	}

}
