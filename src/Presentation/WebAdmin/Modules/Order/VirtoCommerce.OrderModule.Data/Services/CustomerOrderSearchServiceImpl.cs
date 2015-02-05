using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Converters;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class CustomerOrderSearchServiceImpl : ICustomerOrderSearchService
	{
		private readonly Func<IOrderRepository> _orderRepositoryFactory;
		public CustomerOrderSearchServiceImpl(Func<IOrderRepository> orderRepositoryFactory)
		{
			_orderRepositoryFactory = orderRepositoryFactory;
		}

		#region ICustomerOrderSearchService Members

		public SearchResult Search(SearchCriteria criteria)
		{
			SearchResult retVal = null;
			using (var repository = _orderRepositoryFactory())
			{
				var query = repository.CustomerOrders;
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
					CustomerOrders = query.OrderBy(x => x.CreatedDate)
										  .Skip(criteria.Start)
										  .Take(criteria.Count)
										  .ToArray()
										  .Select(x => x.ToCoreModel())
										  .ToList()
				};
			}
			return retVal;
		}

		#endregion
	}
}
