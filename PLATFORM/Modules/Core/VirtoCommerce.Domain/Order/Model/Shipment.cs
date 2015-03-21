using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Order.Model
{
	public class Shipment : Operation, IStockOutOperation
	{
		public string OrganizationId { get; set; }
		public string FulfillmentCenterId { get; set; }
		public string EmployeeId { get; set; }

		public string CustomerOrderId { get; set; }
		public CustomerOrder CustomerOrder { get; set; }

		public ICollection<LineItem> Items { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public Address DeliveryAddress { get; set; }
		public decimal DiscountAmount
		{
			get
			{
				return Discount != null ? Discount.DiscountAmount : 0;
			}
		}
		public Discount Discount { get; set; }

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
				return retVal;
			}
		}

		#region IStockOperation Members

		public IEnumerable<IPosition> Positions
		{
			get { return Items; }
		}

		#endregion
	}
}
