using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Model
{
	public class Order : AuditableEntityBase<Order>, INumbered
	{
		public Order(string number)
		{
			Number = number;
		}

		public ICollection<Payment> Payments { get; set; }

		#region INumbered Members

		public string Number
		{
			get;
			set;
		}

		#endregion
		public DateTime? AcceptedDate { get; set; }
		public DateTime? ProcessedDate { get; set; }
		public DateTime? CanceledDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public DateTime? ClosedDate { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }
		/// <summary>
		/// Gets or sets the total for the order form including all the discounts. Subtotal + shippingSubTotal + TaxTotal - discountTotal.
		/// </summary>
		/// <value>
		/// The total.
		/// </value>
		public Money Total { get; set; }

		/// <summary>
		/// Gets or sets the shipments total. the sum of all shipment
		/// </summary>
		/// <value>
		/// The shipping total.
		/// </value>
		public Money ShippingTotal { get; set; }

		/// <summary>
		/// Gets or sets the total handling fee.
		/// </summary>
		/// <value>
		/// The handling total.
		/// </value>
		public Money HandlingTotal { get; set; }

		/// <summary>
		/// Gets or sets the sum of all applicable taxes.
		/// </summary>
		/// <value>
		/// The tax total.
		/// </value>
		public Money TaxTotal { get; set; }

		/// <summary>
		/// Gets or sets the sub total. Sum of all LineItems' (ListPrice * Quantity). Doesn't include any discounts, shipment costs, taxes or handling fees.
		/// </summary>
		/// <value>
		/// The sub total.
		/// </value>
		public Money Subtotal { get; set; }

		/// <summary>
		/// Gets or sets the order subtotal discount amount.
		/// </summary>
		/// <value>
		/// The discount amount.
		/// </value>
		public Money DiscountTotal { get; set; }
	
	}
}
