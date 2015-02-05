using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.OrderModule.Data.Workflow
{
	public class CalculateTotalsActivity : IObserver<CustomerOrder>
	{
		#region IObserver<CustomerOrder> Members

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(CustomerOrder value)
		{
			CalculateCustomerOrderTotals(value);
		}

		#endregion
		private static void CalculateCustomerOrderTotals(CustomerOrder order)
		{
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
				CalculateShipmentTotals(shipment);
			}

			if(order.Discount != null)
			{
				order.Sum -= order.Discount.DiscountAmount;
			}

			if (order.TaxIncluded)
			{
				order.Sum += order.Tax;
			}
		}

		private static void CalculateShipmentTotals(Shipment shipment)
		{
			shipment.Sum = 0;
			if (shipment.Items != null)
			{
				foreach (var item in shipment.Items)
				{
					shipment.Sum += item.Price * item.Quantity;
					if (item.Discount != null)
					{
						shipment.Sum -= item.Discount.DiscountAmount;
					}
				}
			}
			if (shipment.Discount != null)
			{
				shipment.Sum -= shipment.Discount.DiscountAmount;
			}

			if (shipment.TaxIncluded)
			{
				shipment.Sum += shipment.Tax;
			}
		}
	}
}
