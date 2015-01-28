using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

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

			if(InPayments != null)
			{
				foreach(var inPayment in InPayments)
				{
					retVal.AddRange(inPayment.GetAllRelatedOperations());
				}
			}

			if(Shipments != null)
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
			this.Sum = 0;
			if (Items != null)
			{
				foreach (var item in Items)
				{
					this.Sum += item.Price * item.Quantity;
					if(item.Discount != null)
					{
						this.Sum -= item.Discount.DiscountAmount;
					}
					else
					{
						this.Sum -= item.StaticDiscount;
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
