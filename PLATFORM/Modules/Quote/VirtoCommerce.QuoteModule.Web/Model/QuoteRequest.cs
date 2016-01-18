using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.QuoteModule.Web.Model
{
    /// <summary>
    ///  Request for quotation (RFQ) is a standard business process whose purpose is to invite suppliers into a
    ///  bidding process to bid on specific products or services. 
    /// </summary>
    public class QuoteRequest : AuditableEntity
	{

        /// <summary>
        /// Unique user friendly document number (generate automatically based on special algorithm realization)
        /// </summary>
        public string Number { get; set; }
		public string StoreId { get; set; }
		public string ChannelId { get; set; }
		public bool IsAnonymous { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string OrganizationName { get; set; }
		public string OrganizationId { get; set; }
        /// <summary>
        /// Id employee who responsible for processing RFQ
        /// </summary>
		public string EmployeeId { get; set; }
        /// <summary>
        /// Employee who responsible for processing RFQ
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Date when RFQ will be expired
        /// </summary>
		public DateTime? ExpirationDate { get; set; }
        /// <summary>
        /// Date used for notification
        /// </summary>
        public DateTime? ReminderDate { get; set; }
        /// <summary>
        /// Flag of managing the need to send notifications
        /// </summary>
		public bool EnableNotification { get; set; } 
        /// <summary>
        /// If is set it restrict any changes on RFQ
        /// </summary>
		public bool IsLocked { get; set; }
		public string Status { get; set; }
        /// <summary>
        /// Tag for auxiliary information
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Public comment visible for customer 
        /// </summary>
		public string Comment { get; set; }
        /// <summary>
        /// Private comment not visible to customer
        /// </summary>
		public string InnerComment { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// Resulting totals for selected proposals
        /// </summary>
        public QuoteRequestTotals Totals { get; set; }
		public string Coupon { get; set; }

        /// <summary>
        /// Manual shipping total for quote request
        /// </summary>
        public decimal ManualShippingTotal { get; set; }
        /// <summary>
        /// Manual sub total for quote request
        /// </summary>
        public decimal ManualSubTotal { get; set; }
        /// <summary>
        /// Relative manual discount amount for quote request in percent
        /// </summary>
        public decimal ManualRelDiscountAmount { get; set; }

        /// <summary>
        /// Predefined shipment method used for delivery order created from current RFQ
        /// </summary>
		public ShipmentMethod ShipmentMethod { get; set; }
		public ICollection<Address> Addresses { get; set; }
        /// <summary>
        /// RFQ items
        /// </summary>
		public ICollection<QuoteItem> Items { get; set; }

		public ICollection<QuoteAttachment> Attachments { get; set; }

		public string LanguageCode { get; set; }

		public ICollection<TaxDetail> TaxDetails { get; set; }
	
		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		public string CancelReason { get; set; }
	
        /// <summary>
        /// System property
        /// </summary>
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

        public ICollection<OperationLog> OperationsLog { get; set; }

    }
}
