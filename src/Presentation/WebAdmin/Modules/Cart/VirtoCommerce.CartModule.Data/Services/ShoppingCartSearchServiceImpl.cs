using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Cart.Repositories;
using VirtoCommerce.Domain.Cart.Services;

namespace VirtoCommerce.CartModule.Data.Services
{
	public class ShoppingCartSearchServiceImpl : IShoppingCartSearchService 
	{
		private IShoppingCartRepository _repository;
		public ShoppingCartSearchServiceImpl(IShoppingCartRepository repository)
		{
			_repository = repository;
		}
		#region IShoppingCartSearchService Members

		public SearchResult Search(SearchCriteria criteria)
		{
			var query = _repository.ShoppingCarts;
			if(criteria.CustomerId != null)
			{
				query = query.Where(x => x.CustomerId == criteria.CustomerId);
			}
			if(criteria.SiteId != null)
			{
				query = query.Where(x => x.SiteId == criteria.SiteId);
			}

			var retVal = new SearchResult
			{
				 TotalCount= query.Count(),
 				 ShopingCarts = query.OrderBy(x=>x.CreatedDate).Skip(criteria.Start).Take(criteria.Count).ToList()
			};
			return retVal;
		}

		#endregion
	}
}
