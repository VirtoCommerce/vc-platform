using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Quote.Model
{
	public class QuoteRequest : AuditableEntity, IHaveTaxDetalization, ISupportCancellation, IHasDynamicProperties, ILanguageSupport, IHasChangesHistory
    {
 		public string Number { get; set; }
		public string StoreId { get; set; }
		public string ChannelId { get; set; }
		public bool IsAnonymous { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string OrganizationName { get; set; }
		public string OrganizationId { get; set; }

		public string EmployeeId { get; set; }
		public string EmployeeName { get; set; }

		public DateTime? ExpirationDate { get; set; }
		public DateTime? ReminderDate { get; set; }

		public bool EnableNotification { get; set; } 
		public bool IsLocked { get; set; }
		public string Status { get; set; }

        public string Tag { get; set; }

       	public string Comment { get; set; }
		public string InnerComment { get; set; }
		public string Currency { get; set; }

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

        public QuoteRequestTotals Totals { get; set; }

		public ShipmentMethod ShipmentMethod { get; set; }
		public ICollection<Address> Addresses { get; set; }
		public ICollection<QuoteItem> Items { get; set; }
		public ICollection<QuoteAttachment> Attachments { get; set; }

        #region IHasChangesHistory Members
        public ICollection<OperationLog> OperationsLog { get; set; } 
        #endregion

        #region ILanguageSupport Members
        public string LanguageCode { get; set; }
		#endregion

		#region IHaveTaxDetalization Members
		public ICollection<TaxDetail> TaxDetails { get; set; }
		#endregion

		#region ISupportCancelation Members
		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		public string CancelReason { get; set; }
		#endregion

		#region IHasDynamicProperties Members
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
		#endregion
	}
}
