using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Order.Model
{
	public class CustomerOrder : Operation
	{
		public ICollection<Address> Addresses { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }

		public ICollection<LineItem> Items { get; set; }
		public ICollection<Shipment> Shipments { get; set; }

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

			if (Shipments != null)
			{
				foreach (var shipment in Shipments)
				{
					retVal.AddRange(shipment.GetAllRelatedOperations());
				}
			}
			return retVal;
		}

		public virtual void CalculateTotals()
		{
			Sum = 0;
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

			if (Shipments != null)
			{
				foreach (var shipment in Shipments)
				{
					shipment.CalculateTotals();
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
