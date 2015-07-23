using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Order.Model
{
    public class CustomerOrder : Operation, IHaveTaxDetalization
	{
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string ChannelId { get; set; }
		public string StoreId { get; set; }
		public string StoreName { get; set; }
		public string OrganizationId { get; set; }
		public string OrganizationName { get; set; }
		public string EmployeeId { get; set; }
		public string EmployeeeName { get; set; }

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
                        inPayment.ParentOperationId = Id;
						retVal.Add(inPayment);
					}
				}

				if (Shipments != null)
				{
					foreach (var shipment in Shipments)
					{
                        shipment.ParentOperationId = Id;
						retVal.Add(shipment);
					}
				}
				return retVal;
			}
		}

   
		#region ITaxDetailSupport Members

		public ICollection<TaxDetail> TaxDetails { get; set; }

		#endregion
	}
}
