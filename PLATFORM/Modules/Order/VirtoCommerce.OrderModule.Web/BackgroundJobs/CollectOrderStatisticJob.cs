using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.OrderModule.Web.BackgroundJobs
{
	public class CollectOrderStatisticJob
	{
		private readonly Func<IOrderRepository> _repositoryFactory;
		private readonly CacheManager _cacheManager;

		internal CollectOrderStatisticJob()
		{
		}

		public CollectOrderStatisticJob(Func<IOrderRepository> repositoryFactory, CacheManager cacheManager)
		{
			_repositoryFactory = repositoryFactory;
			_cacheManager = cacheManager;
		}

		public DashboardStatisticsResult CollectStatistics(DateTime start, DateTime end)
		{
			var retVal = new DashboardStatisticsResult();
		
			using (var repository = _repositoryFactory())
			{
				retVal.OrderCount = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
																.Where(x => !x.IsCancelled).Count();
				//avg order value
				var avgValues = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
														 .GroupBy(x => x.Currency)
														 .Select(x => new { Currency = x.Key, AvgValue = x.Average(y => y.Sum) })
														 .ToArray();
				retVal.AvgOrderValue = avgValues.Select(x=> new Money(x.Currency, x.AvgValue) ).ToList();


				//Revenue
				var revenues = repository.InPayments.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
													.Where(x => !x.IsCancelled)
													.GroupBy(x => x.Currency).Select(x => new { Currency = x.Key, Value = x.Sum(y => y.Sum) })
													.ToArray();
				retVal.Revenue = revenues.Select(x => new Money(x.Currency, x.Value)).ToList();

				var currencies = repository.InPayments.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
												.Where(x => !x.IsCancelled)
												.GroupBy(x => x.Currency).Select(x => x.Key);

				retVal.RevenuePeriodDetails = new List<MoneyWithPeriod>();
				foreach (var currency in currencies)
				{
					for (var currentDate = start; currentDate <= end; currentDate = currentDate.AddMonths(1))
					{
						var currentEndDate = currentDate.AddMonths(1);
						var quarter = (int)(currentDate.Month / 3);
						var amount = repository.InPayments.Where(x => x.CreatedDate >= currentDate && x.CreatedDate <= currentEndDate)
															  .Where(x => !x.IsCancelled & x.Currency == currency).Sum(x => (decimal?)x.Sum) ?? 0;
						var periodStat = new MoneyWithPeriod(currency, amount)
						{
							Month = currentDate.Month,
							Quarter = quarter,
							Year = currentDate.Year
						};
						retVal.RevenuePeriodDetails.Add(periodStat);


					}
				}

				//RevenuePerCustomer
				var revenuesPerCustomer = repository.InPayments.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
															   .Where(x => !x.IsCancelled).GroupBy(x => x.Currency)
															   .Select(x => new { Currency = x.Key, AvgValue = x.GroupBy(y => y.CustomerId).Average(y => y.Sum(z => z.Sum)) })
															   .ToArray();
				retVal.RevenuePerCustomer = revenuesPerCustomer.Select(x => new Money(x.Currency, x.AvgValue)).ToList();

				//Items purchased
				retVal.ItemsPurchased = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
																	.Where(x => !x.IsCancelled).SelectMany(x => x.Items).Count();

				//Line items per order
				retVal.LineitemsPerOrder = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
																 .Where(x => !x.IsCancelled).Select(x=>x.Items.Count()).DefaultIfEmpty(0).Average();

				//Customer count
				retVal.CustomersCount = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
																	.Select(x => x.CustomerId).Distinct().Count();

			}
			retVal.StartDate = start;
			retVal.EndDate = end;
			return retVal;
		}
	}
}