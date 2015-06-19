using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.OrderModule.Web.Model
{
    public class DashboardStatisticsResult
    {
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public ICollection<Money> Revenue { get; set; }
		public ICollection<QuarterPeriodMoney> RevenuePeriodDetails { get; set; }
		public int OrderCount { get; set; }
		public int CustomersCount { get; set; }
		public ICollection<Money> RevenuePerCustomer { get; set; }
		public ICollection<Money> AvgOrderValue { get; set; }
		public ICollection<QuarterPeriodMoney> AvgOrderValuePeriodDetails { get; set; }

		public int ItemsPurchased { get; set; }
		public double LineitemsPerOrder { get; set; }
    }

	public class Money
	{
		public Money(string currency, decimal amount)
		{
			Currency = currency;
			Amount = amount;
		}
		public string Currency { get; set; }
		public decimal Amount { get; set; }

	}


	public class QuarterPeriodMoney : Money
	{
		public QuarterPeriodMoney(string currency, decimal amount)
			:base(currency, amount)
		{
		}
		public int Year { get; set; }
		public int Quarter { get; set; }

	}
}
