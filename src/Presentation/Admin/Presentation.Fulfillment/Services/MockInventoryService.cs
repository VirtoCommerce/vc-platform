using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Inventories.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.Services
{
	public class MockInventoryService: IInventoryRepository 
	{
		private IList[] MockLists;

		private List<Inventory> InventoryList = new List<Inventory>();

		public MockInventoryService()
		{
			InventoryList.Add(new Inventory { InventoryId = "111", FulfillmentCenterId = "vendor-fulfillment", AllowBackorder = true, AllowPreorder = true, BackorderAvailabilityDate = DateTime.Now.AddDays(-5), BackorderQuantity = 5, Created = DateTime.Now.AddDays(-50), InStockQuantity = 20, PreorderAvailabilityDate = DateTime.Now.AddDays(-10), PreorderQuantity = 10, ReorderMinQuantity = 3, ReservedQuantity = 2, Sku = "v-1223", Status = 1 });

			MockLists = new IList[] { InventoryList };
		}

		#region IInventoryRepository

		public IQueryable<Inventory> Inventories
		{
			get { return InventoryList.AsQueryable(); }
		}

		#endregion

		#region IRepository Members

		MockUnitOfWork MockUnitOfWorkItem = new MockUnitOfWork();
		public IUnitOfWork UnitOfWork
		{
			get { return MockUnitOfWorkItem; }
		}

		public void Attach<T>(T item) where T : class
		{
		}

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }


		public void Add<T>(T item) where T : class
		{
			var list = MockLists.OfType<List<T>>().First();

			if (!list.Contains(item))
				list.Add(item);
		}

		public void Update<T>(T item) where T : class
		{
		}

		public void Remove<T>(T item) where T : class
		{
			var list = MockLists.OfType<List<T>>().First();
			list.Remove(item);
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			throw new NotImplementedException();
		}

	    public void Refresh(IEnumerable collection)
	    {
	        throw new NotImplementedException();
	    }

	    #endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion

	}

	public class MockUnitOfWork : IUnitOfWork
	{
		public int Commit()
		{
			return 0;
		}

		public void CommitAndRefreshChanges()
		{
		}

		public void RollbackChanges()
		{
		}
	}

}
