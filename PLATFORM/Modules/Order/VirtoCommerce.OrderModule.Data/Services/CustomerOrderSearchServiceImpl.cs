using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Converters;
using System.Data.Entity;

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

				if ((criteria.ResponseGroup & ResponseGroup.WithDiscounts) == ResponseGroup.WithDiscounts)
				{
					query = query.Include(x => x.Discounts);
				}

				if ((criteria.ResponseGroup & ResponseGroup.WithAddresses) == ResponseGroup.WithAddresses)
				{
					query = query.Include(x => x.Addresses);
				}

				if ((criteria.ResponseGroup & ResponseGroup.WithItems) == ResponseGroup.WithItems)
				{
					query = query.Include(x => x.Items);

					if ((criteria.ResponseGroup & ResponseGroup.WithDiscounts) == ResponseGroup.WithDiscounts)
					{
						query = query.Include(x => x.Items.Select(y=>y.Discounts));
					}
				}

				if ((criteria.ResponseGroup & ResponseGroup.WithInPayments) == ResponseGroup.WithInPayments)
				{
					query = query.Include(x => x.InPayments);
				}
				if ((criteria.ResponseGroup & ResponseGroup.WithShipments) == ResponseGroup.WithShipments)
				{
					query = query.Include(x => x.Shipments);

					if ((criteria.ResponseGroup & ResponseGroup.WithDiscounts) == ResponseGroup.WithDiscounts)
					{
						query = query.Include(x => x.Items.Select(y => y.Discounts));
					}

					if ((criteria.ResponseGroup & ResponseGroup.WithAddresses) == ResponseGroup.WithAddresses)
					{
						query = query.Include(x => x.Shipments.Select(y=>y.Addresses));
					}
				}
			
				retVal = new SearchResult
				{
					TotalCount = query.Count(),
					CustomerOrders = query.OrderByDescending(x => x.CreatedDate)
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
