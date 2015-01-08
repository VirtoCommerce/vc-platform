using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Repositories;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.CartModule.Data.Converters;

namespace VirtoCommerce.CartModule.Data.Services
{
	public class ShoppingCartServiceImpl : IShoppingCartService
	{
		private IShoppingCartRepository _repository;
		public ShoppingCartServiceImpl(IShoppingCartRepository repository)
		{
			_repository = repository;
		}
		#region IShoppingCartService Members

		public Domain.Cart.Model.ShoppingCart GetById(string cartId)
		{
			return _repository.GetById(cartId);
		}

		public Domain.Cart.Model.ShoppingCart Create(Domain.Cart.Model.ShoppingCart cart)
		{
			cart.CalculateTotals();

			_repository.Add(cart);
			
			_repository.UnitOfWork.Commit();
			return cart;
		}

		public void Update(Domain.Cart.Model.ShoppingCart[] carts)
		{
			foreach(var sourceCart in carts)
			{
				var targetCart = GetById(sourceCart.Id);
				sourceCart.Patch(targetCart);

				targetCart.CalculateTotals();

				_repository.UnitOfWork.Commit();
			}
		}

		public void Delete(string[] cartIds)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
