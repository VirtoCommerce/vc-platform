using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.OrderModule.Web.Model
{
	/// <summary>
	/// Represent customer order shipment operation (document)
	/// contains information as delivery address, items, dimensions etc.
	/// </summary>
	public class Shipment : Operation, IHaveTaxDetalization
	{
		/// <summary>
		/// Customer organization
		/// </summary>
		public string OrganizationName { get; set; }
		public string OrganizationId { get; set; }
		/// <summary>
		/// Fulfillment center where shipment will be handled
		/// </summary>
		public string FulfillmentCenterName { get; set; }
		public string FulfillmentCenterId { get; set; }
	
        /// <summary>
        /// Selected shipping method to deliver current shipment
        /// </summary>
        public ShippingMethod ShippingMethod { get; set; }

		/// <summary>
		/// Employee who responsible for handling current shipment
		/// </summary>
		public string EmployeeName { get; set; }
		public string EmployeeId { get; set; }
		public decimal DiscountAmount { get; set; }

		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		public string TaxType { get; set; }

		/// <summary>
		/// Information about quantity and order items belongs to current shipment
		/// </summary>
		public ICollection<ShipmentItem> Items { get; set; }
		/// <summary>
		/// Information about packages belongs to current shipment
		/// </summary>
		public ICollection<ShipmentPackage> Packages { get; set; }
		public ICollection<PaymentIn> InPayments { get; set; }
		public Address DeliveryAddress { get; set; }
		public Discount Discount { get; set; }

		
		#region IHaveTaxDetalization Members
		public ICollection<TaxDetail> TaxDetails { get; set; }
		#endregion
	}
}