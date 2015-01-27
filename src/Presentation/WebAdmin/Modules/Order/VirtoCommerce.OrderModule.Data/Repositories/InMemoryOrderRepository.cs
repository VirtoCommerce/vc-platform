using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
	public class InMemoryOrderRepository : IOrderRepository, IUnitOfWork
	{
		private List<CustomerOrder> _customerOrders = new List<CustomerOrder>();
		#region IOrderRepository Members

		public IQueryable<CustomerOrder> CustomerOrders
		{
			get { return _customerOrders.AsQueryable(); }
		}

		public IQueryable<Shipment> Shipments
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<PaymentIn> InPayments
		{
			get { throw new NotImplementedException(); }
		}

		public CustomerOrder GetCustomerOrderById(string id, ResponseGroup responseGroup)
		{
			return _customerOrders.FirstOrDefault(x => x.Id == id);
		}

		public CustomerOrder GetShipmentById(string id, ResponseGroup responseGroup)
		{
			throw new NotImplementedException();
		}

		public CustomerOrder GetInPaymentById(string id, ResponseGroup responseGroup)
		{
			throw new NotImplementedException();
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
			_customerOrders.Add(item as CustomerOrder);
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
