using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.OrderModule.Web.Model
{
	/// <summary>
	/// Represent customer order
	/// </summary>
	public class CustomerOrder : Operation, IHaveTaxDetalization
	{
		public string CustomerName { get; set; }
		public string CustomerId { get; set; }
		/// <summary>
		/// Chanel (Web site, mobile application etc)
		/// </summary>
		public string ChannelId { get; set; }
		public string StoreId { get; set; }
		public string StoreName { get; set; }
		public string OrganizationName { get; set; }
		public string OrganizationId { get; set; }
		/// <summary>
		/// Employee who should handle that order
		/// </summary>
		public string EmployeeName { get; set; }
		public string EmployeeId { get; set; }
		public decimal DiscountAmount { get; set; }	

		/// <summary>
		/// All shipping and billing order addresses
		/// </summary>
		public ICollection<Address> Addresses { get; set; }
		/// <summary>
		/// Incoming payments operations
		/// </summary>
		public ICollection<PaymentIn> InPayments { get; set; }
		/// <summary>
		/// All customer order line items
		/// </summary>
		public ICollection<LineItem> Items { get; set; }
		/// <summary>
		/// All customer order shipments
		/// </summary>
		public ICollection<Shipment> Shipments { get; set; }
		/// <summary>
		/// All customer order discount
		/// </summary>
		public Discount Discount { get; set; }

		#region IHaveTaxDetalization Members
		/// <summary>
		/// Tax details
		/// </summary>
		public ICollection<TaxDetail> TaxDetails { get; set; }
		#endregion
        /// <summary>
        /// Security permission scopes used for security check on UI
        /// </summary>
        public string[] Scopes { get; set; }

       
	}
}