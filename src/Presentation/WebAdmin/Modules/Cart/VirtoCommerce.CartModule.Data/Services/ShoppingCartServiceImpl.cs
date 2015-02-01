using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Converters;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;

namespace VirtoCommerce.CartModule.Data.Services
{
	public class ShoppingCartServiceImpl : ModuleServiceBase, IShoppingCartService
	{
		private Func<ICartRepository> _repositoryFactory;
		public ShoppingCartServiceImpl(Func<ICartRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}
		#region IShoppingCartService Members

		public ShoppingCart GetById(string cartId)
		{
			ShoppingCart retVal = null;
			using (var repository = _repositoryFactory())
			{
				var entity = repository.GetShoppingCartById(cartId);
				if (entity != null)
				{
					retVal = entity.ToCoreModel();
				}
			}
			return retVal;
		}

		public ShoppingCart Create(ShoppingCart cart)
		{
			cart.CalculateTotals();

			var entity = cart.ToEntity();
			ShoppingCart retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
			}
			retVal = GetById(entity.Id);
			return retVal;
		}

		public void Update(ShoppingCart[] carts)
		{
			var changedCarts = new List<ShoppingCart>();
		
			//foreach (var cart in carts)
			//{
			//	//Apply changes to temporary  object
			//	var targetCart = GetById(cart.Id);
			//	if (targetCart == null)
			//	{
			//		throw new NullReferenceException("targetCart");
			//	}
			//	var sourceCartEntity = cart.ToEntity();
			//	var targetCartEntity = targetCart.ToEntity();
			//	sourceCartEntity.Patch(targetCartEntity);
			//	var changedCart = targetCartEntity.ToCoreModel();
			//	changedCarts.Add(changedCart);
			//}


			//Need a call business logic for changes and persist changes
			using (var repository = _repositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var changedCart in carts)
				{
					//Do business logic on temporary  order object
					changedCart.CalculateTotals();

					var sourceCartEntity = changedCart.ToEntity();
					var targetCartEntity = repository.GetShoppingCartById(changedCart.Id);
					if (targetCartEntity == null)
					{
						throw new NullReferenceException("targetCartEntity");
					}

					changeTracker.Attach(targetCartEntity);
					sourceCartEntity.Patch(targetCartEntity);
				}
				CommitChanges(repository);
			}
	
		}

		public void Delete(string[] cartIds)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
