using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Converters;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;

namespace VirtoCommerce.CartModule.Data.Services
{
	public class ShoppingCartServiceImpl : ServiceBase, IShoppingCartService
	{
		private const string _workflowName = "CartRecalculate";
		private Func<ICartRepository> _repositoryFactory;
		private readonly IWorkflowService _workflowService;
		public ShoppingCartServiceImpl(Func<ICartRepository> repositoryFactory, IWorkflowService workflowService)
		{
			_repositoryFactory = repositoryFactory;
			_workflowService = workflowService;
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

					RecalculateCart(retVal);
				}
			}

			return retVal;
		}

		public ShoppingCart Create(ShoppingCart cart)
		{

			//Do business logic on temporary  order object
			RecalculateCart(cart);

			var entity = cart.ToEntity();
			ShoppingCart retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
			}
			retVal = GetById(entity.Id);

			RecalculateCart(retVal);

			return retVal;
		}

		public void Update(ShoppingCart[] carts)
		{
			var changedCarts = new List<ShoppingCart>();

			foreach (var cart in carts)
			{
				//Apply changes to temporary  object
				var targetCart = GetById(cart.Id);
				if (targetCart == null)
				{
					throw new NullReferenceException("targetCart");
				}
				var sourceCartEntity = cart.ToEntity();
				var targetCartEntity = targetCart.ToEntity();
				sourceCartEntity.Patch(targetCartEntity);
				var changedCart = targetCartEntity.ToCoreModel();
				changedCarts.Add(changedCart);
			}


			//Need a call business logic for changes and persist changes
			using (var repository = _repositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var changedCart in changedCarts)
				{
					//Do business logic on temporary  order object
					RecalculateCart(changedCart);

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

		private void RecalculateCart(ShoppingCart cart)
		{
			var parameters = new Dictionary<string, object>();
			parameters["cart"] = cart;
			_workflowService.RunWorkflow(_workflowName, parameters, null);
		}
	}
}
