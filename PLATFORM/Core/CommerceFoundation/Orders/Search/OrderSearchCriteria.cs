using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Orders.Search
{
	[DataContract]
	public class OrderSearchCriteria : SearchCriteriaBase
	{
		/// <summary>
		/// Gets or sets the store codes.
		/// </summary>
		/// <value>
		/// The store id.
		/// </value>
		[DataMember]
		public string[] StoreIds { get; set; }

		[DataMember]
		public string OrganizationId { get; set; }

		[DataMember]
		public string TrackingNumber { get; set; }

		[DataMember]
		public string CustomerEmail { get; set; }

		[DataMember]
		public string Keyword { get; set; }

		/// <summary>
		/// Gets or sets the customer id.
		/// </summary>
		/// <value>
		/// The customer id.
		/// </value>
		[DataMember]
		public string CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the company id.
		/// </summary>
		/// <value>
		/// The company id.
		/// </value>
		[DataMember]
		public string CompanyId { get; set; }

		/// <summary>
		/// Gets or sets the order status.
		/// </summary>
		/// <value>
		/// The order status.
		/// </value>
		[DataMember]
		public string OrderStatus { get; set; }

		/// <summary>
		/// Gets or sets the order date.
		/// </summary>
		/// <value>
		/// The order date.
		/// </value>
		[DataMember]
		public DateTime OrderDate { get; set; }

		/// <summary>
		/// Gets or sets the shipment status.
		/// </summary>
		/// <value>
		/// The shipment status.
		/// </value>
		[DataMember]
		public string ShipmentStatus { get; set; }

		/// <summary>
		/// Gets or sets the catalog item code.
		/// </summary>
		/// <value>
		/// The catalog item code.
		/// </value>
		[DataMember]
		public string CatalogItemCode { get; set; }

		/// <summary>
		/// Gets or sets the shipment postal code.
		/// </summary>
		/// <value>
		/// The shipment postal code.
		/// </value>
		[DataMember]
		public string ShipmentPostalCode { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderSearchCriteria"/> class.
		/// </summary>
		/// <param name="scope">The scope.</param>
		public OrderSearchCriteria()
			: base("order")
		{

		}
	}
}