using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.OrderModule.Data.Model;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
	public class InMemoryOrderRepository : IOrderRepository, IUnitOfWork
	{
		private List<CustomerOrderEntity> _customerOrders = new List<CustomerOrderEntity>();
		#region IOrderRepository Members

		public IQueryable<CustomerOrderEntity> CustomerOrders
		{
			get { return _customerOrders.AsQueryable(); }
		}

		public IQueryable<ShipmentEntity> Shipments
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<PaymentInEntity> InPayments
		{
			get { throw new NotImplementedException(); }
		}

		public CustomerOrderEntity GetCustomerOrderById(string id, CustomerOrderResponseGroup responseGroup)
		{
			return _customerOrders.FirstOrDefault(x => x.Id == id);
		}

		#endregion

		#region IRepository Members

		public IUnitOfWork UnitOfWork
		{
			get { return this; }
		}

		public void Attach<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public bool IsAttachedTo<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public void Add<T>(T item) where T : class
		{
			_customerOrders.Add(item as CustomerOrderEntity);
		}

		public void Update<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public void Remove<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			throw new NotImplementedException();
		}

		public void Refresh(System.Collections.IEnumerable collection)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		
		}

		#endregion

		#region IUnitOfWork Members

		public int Commit()
		{
			return -1;
		}

		public void CommitAndRefreshChanges()
		{
			throw new NotImplementedException();
		}

		public void RollbackChanges()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
