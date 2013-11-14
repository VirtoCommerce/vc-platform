using System;
using System.Activities;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{
	/// <summary>
	/// Calculates totals for a RMA request in the order object based on the values entered for line items as well as discounts.
	/// </summary>
	public class CalculateReturnTotalsActivity : OrderActivityBase
	{
		protected override void Execute(CodeActivityContext context)
		{
			base.Execute(context);

			var orderItem = CurrentOrderGroup as Order;
			if (orderItem == null)
			{
				return;
			}

			var calculateItem = orderItem.RmaRequests
				.FirstOrDefault(x => !x.Created.HasValue &&
					x.Status == RmaRequestStatus.AwaitingStockReturn.ToString());

			CalculateTotals(calculateItem);
		}

		// Calculate totals for RmaRequest
		private void CalculateTotals(RmaRequest item)
		{
			decimal itemTax = 0m;
			foreach (var returnItem in item.RmaReturnItems)
			{
				var lineItem = returnItem.RmaLineItems[0].LineItem;
				var amountTmp = lineItem.ExtendedPrice * returnItem.RmaLineItems[0].ReturnQuantity / lineItem.Quantity;
				returnItem.ReturnAmount = Math.Floor(amountTmp * 100) * 0.01m;
				amountTmp = lineItem.TaxTotal * returnItem.RmaLineItems[0].ReturnQuantity / lineItem.Quantity;
				itemTax += Math.Floor(amountTmp * 100) * 0.01m;
			}
			item.ItemSubtotal = item.RmaReturnItems.Sum(x => x.ReturnAmount);

			decimal ShippingCost = 0;
			decimal ShippingTax = 0;

			/// TODO: ShippingCost
			// ShippingCost = ??? // take LineItems, addresses, shipping methods and... calculate it.
			// ShippingTax = 

			item.ItemTax = itemTax;
			// exclude taxes and subtract shipping costs
			item.TotalBeforeTax = item.ItemSubtotal - item.ItemTax - ShippingCost;
			item.LessReStockingFee = ShippingCost + ShippingTax;
			item.ReturnTotal = item.ItemSubtotal - item.LessReStockingFee;
			item.RefundAmount = 0m;
		}
	}
}
