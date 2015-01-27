using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CartModule.Data.Repositories
{
	public class InMemoryCartRepository : IShoppingCartRepository, IUnitOfWork
	{
		private List<ShoppingCart> _carts = new List<ShoppingCart>();

		#region IShoppingCartRepository Members

		public IQueryable<ShoppingCart> ShoppingCarts
		{
			get { return _carts.AsQueryable(); }
		}

		public ShoppingCart GetById(string id)
		{
			return _carts.FirstOrDefault(x=>x.Id == id);
		}

		#endregion

		#region IRepository Members

		public Foundation.Frameworks.IUnitOfWork UnitOfWork
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
			_carts.Add(item as ShoppingCart);
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
			return 0;
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
