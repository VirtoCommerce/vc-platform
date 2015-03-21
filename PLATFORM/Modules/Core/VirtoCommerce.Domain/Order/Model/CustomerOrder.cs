using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Order.Model
{
	public class CustomerOrder : Operation
	{
		public string CustomerId { get; set; }
		public string ChannelId { get; set; }
		public string StoreId { get; set; }
		public string OrganizationId { get; set; }
		public string EmployeeId { get; set; }

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		public string CancelReason { get; set; }

		public ICollection<Address> Addresses { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }

		public ICollection<LineItem> Items { get; set; }
		public ICollection<Shipment> Shipments { get; set; }

		public Discount Discount { get; set; }

		public decimal DiscountAmount
		{
			get
			{
				return Discount != null ? Discount.DiscountAmount : 0;
			}
		}

		public override IEnumerable<Operation> ChildrenOperations
		{
			get
			{
				var retVal = new List<Operation>();

				if (InPayments != null)
				{
					foreach (var inPayment in InPayments)
					{
						inPayment.ParentOperationId = this.Id;
						retVal.Add(inPayment);
					}
				}

				if (Shipments != null)
				{
					foreach (var shipment in Shipments)
					{
						shipment.ParentOperationId = this.Id;
						retVal.Add(shipment);
					}
				}
				return retVal;
			}
		}
	}
}
