using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.OrderModule.Data.Observers
{
	public class CalculateTotalsObserver : IObserver<OrderChangeEvent>
	{
		#region IObserver<CustomerOrder> Members

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(OrderChangeEvent value)
		{
			CalculateCustomerOrderTotals(value);
		}

		#endregion
		private static void CalculateCustomerOrderTotals(OrderChangeEvent context)
		{
			var order = context.ModifiedOrder;

			order.Sum = 0;

			foreach (var item in order.Items)
			{
				order.Sum += item.Price * item.Quantity;
				if(item.Discount != null)
				{
					order.Sum -= item.Discount.DiscountAmount;
				}
			}

			foreach (var shipment in order.Shipments)
			{
				order.Sum += shipment.Sum - shipment.DiscountAmount + shipment.Tax;
			}
	
			if (order.TaxIncluded)
			{
				order.Sum += order.Tax;
			}
			order.Sum -= order.DiscountAmount;
		}

		
	}
}
