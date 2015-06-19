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
				var currencies = repository.InPayments.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
										.Where(x => !x.IsCancelled)
										.GroupBy(x => x.Currency).Select(x => x.Key);

				retVal.OrderCount = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
																.Where(x => !x.IsCancelled).Count();
				//avg order value
				var avgValues = repository.CustomerOrders.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
														 .GroupBy(x => x.Currency)
														 .Select(x => new { Currency = x.Key, AvgValue = x.Select(y=>y.Sum).DefaultIfEmpty(0).Average() })
														 .ToArray();
				retVal.AvgOrderValue = avgValues.Select(x=> new Money(x.Currency, x.AvgValue) ).ToList();

			
				//Revenue
				var revenues = repository.InPayments.Where(x => x.CreatedDate >= start && x.CreatedDate <= end)
													.Where(x => !x.IsCancelled)
													.GroupBy(x => x.Currency).Select(x => new { Currency = x.Key, Value = x.Select(y=>y.Sum).DefaultIfEmpty(0).Sum() })
													.ToArray();
				retVal.Revenue = revenues.Select(x => new Money(x.Currency, x.Value)).ToList();


				retVal.RevenuePeriodDetails = new List<QuarterPeriodMoney>();
				retVal.AvgOrderValuePeriodDetails = new List<QuarterPeriodMoney>();
				var endDate = end;
				foreach (var currency in currencies)
				{
					for (var startDate = start; startDate < end; startDate = endDate)
					{
						endDate = startDate.AddMonths(3 - ((startDate.Month - 1)% 3));
						endDate = new DateTime(endDate.Year, endDate.Month, 1);
						endDate = new DateTime(Math.Min(end.Ticks, endDate.Ticks));
						var quarter = (int)((startDate.Month - 1) / 3) + 1;
						
						var amount = repository.InPayments.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
														  .Where(x => !x.IsCancelled & x.Currency == currency).Select(x=>x.Sum).DefaultIfEmpty(0).Sum();
						var avgOrderValue = repository.CustomerOrders.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
														 .Where(x => x.Currency == currency)
														 .Select(x=>x.Sum).DefaultIfEmpty(0).Average();

						var periodStat = new QuarterPeriodMoney(currency, amount)
						{
							Quarter = quarter,
							Year = startDate.Year
						};
						retVal.RevenuePeriodDetails.Add(periodStat);

						periodStat = new QuarterPeriodMoney(currency, avgOrderValue)
						{
							Quarter = quarter,
							Year = startDate.Year
						};
						retVal.AvgOrderValuePeriodDetails.Add(periodStat);


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