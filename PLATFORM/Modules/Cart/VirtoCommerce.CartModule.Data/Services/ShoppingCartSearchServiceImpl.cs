using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.CartModule.Data.Converters;
using System.Collections.Concurrent;

namespace VirtoCommerce.CartModule.Data.Services
{
	public class ShoppingCartSearchServiceImpl : IShoppingCartSearchService
	{
		private Func<ICartRepository> _repositoryFactory;
		public ShoppingCartSearchServiceImpl(Func<ICartRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}
		#region IShoppingCartSearchService Members

		public SearchResult Search(SearchCriteria criteria)
		{
			SearchResult retVal = null;
			var cartIds = new List<string>();
			using (var repository = _repositoryFactory())
			{
				var query = repository.ShoppingCarts;
				if (criteria.CustomerId != null)
				{
					query = query.Where(x => x.CustomerId == criteria.CustomerId);
				}
				if (criteria.StoreId != null)
				{
					query = query.Where(x => x.StoreId == criteria.StoreId);
				}

				retVal = new SearchResult
				{
					TotalCount = query.Count(),

				};
				cartIds = query.OrderBy(x => x.CreatedDate)
										  .Skip(criteria.Start)
										  .Take(criteria.Count)
										  .Select(x => x.Id).ToList();

			}
			var carts = new ConcurrentBag<ShoppingCart>();
			var parallelOptions = new ParallelOptions
		   {
			   MaxDegreeOfParallelism = 10
		   };

			Parallel.ForEach(cartIds, parallelOptions, (x) =>
			{
				using (var repository = _repositoryFactory())
				{
					var cart = repository.GetShoppingCartById(x);
					carts.Add(cart.ToCoreModel());
				}
			});

			retVal.ShopingCarts = carts.ToList();
			return retVal;
		}

		#endregion
	}
}
