using DotLiquid;
using System;
using System.Collections.Generic;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class QuoteRequest : Drop
    {
        public string Id { get; set; }

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

        public Currency Currency { get; set; }

        public QuoteRequestTotals Totals { get; set; }

        public string Coupon { get; set; }

        public decimal ManualShippingTotal { get; set; }

        public decimal ManualSubTotal { get; set; }

        public decimal ManualRelDiscountAmount { get; set; }

        public ShippingMethod ShipmentMethod { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<QuoteItem> Items { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public Language Language { get; set; }

        public ICollection<TaxLine> TaxDetails { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string CancelReason { get; set; }

        public string ObjectType { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}