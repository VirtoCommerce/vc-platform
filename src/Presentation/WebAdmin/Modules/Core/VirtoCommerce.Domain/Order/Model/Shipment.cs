using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Order.Model
{
	public class Shipment : CommingOutOperation
	{
		public string OrganizationId { get; set; }
		public string FulfilmentCenterId { get; set; }
		public string EmployeeId { get; set; }

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
					inPayment.ParentOperationId = this.Id;
					retVal.AddRange(inPayment.GetAllRelatedOperations());
				}
			}
			return retVal;
		}

		public virtual void CalculateTotals()
		{
			this.Sum = 0;
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

			if (Discount != null)
			{
				this.Sum -= Discount.DiscountAmount;
			}

			if (TaxIncluded ?? false)
			{
				this.Sum += this.Tax;
			}
		}
	}
}
