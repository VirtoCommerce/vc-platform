using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					this.Sum += item.Price * item.Quantity;
					if (item.Discount != null)
					{
						this.Sum -= item.Discount.DiscountAmount;
					}
					else
					{
						this.Sum -= item.StaticDiscount;
					}
				}
			}

			if (Discount != null && Discount.DiscountAmount != null)
			{
				this.Sum -= Discount.DiscountAmount;
			}

			if (this.TaxIncluded ?? false)
			{
				this.Sum += this.Tax;
			}
		}
	}
}
