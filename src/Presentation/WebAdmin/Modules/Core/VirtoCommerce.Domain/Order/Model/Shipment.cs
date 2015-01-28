using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Order.Model
{
	public class Shipment : CommingOutOperation
	{
		public string CustomerOrderId { get; set; }
		public CustomerOrder CustomerOrder { get; set; }

		public ICollection<LineItem> Items { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public Address DeliveryAddress { get; set; }
		public Discount Discount { get; set; }

		public override IEnumerable<Operation> GetAllRelatedOperations()
		{
			var retVal = base.GetAllRelatedOperations().ToList();

			if (InPayments != null)
			{
				foreach (var inPayment in InPayments)
				{
					retVal.AddRange(inPayment.GetAllRelatedOperations());
				}
			}
			return retVal;
		}

		public virtual void CalculateTotals()
		{
			if (Items != null)
			{
				foreach (var item in Items)
				{
					Sum += item.Price * item.Quantity;
					if (item.Discount != null)
					{
						Sum -= item.Discount.DiscountAmount;
					}
					else
					{
						Sum -= item.StaticDiscount;
					}
				}
			}

			if (Discount != null)
			{
				Sum -= Discount.DiscountAmount;
			}

			if (TaxIncluded ?? false)
			{
				Sum += Tax;
			}
		}
	}
}
