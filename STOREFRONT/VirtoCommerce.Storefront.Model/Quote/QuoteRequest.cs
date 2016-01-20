﻿using System;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteRequest : Entity
    {
        public QuoteRequest()
        {
            Addresses = new List<Address>();
            Attachments = new List<Attachment>();
            Items = new List<QuoteItem>();
            TaxDetails = new List<TaxDetail>();
            DynamicProperties = new List<DynamicProperty>();
        }

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

        public Coupon Coupon { get; set; }

        public Money ManualShippingTotal { get; set; }

        public Money ManualSubTotal { get; set; }

        public Money ManualRelDiscountAmount { get; set; }

        public ShippingMethod ShipmentMethod { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<QuoteItem> Items { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public Language Language { get; set; }

        public ICollection<TaxDetail> TaxDetails { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string CancelReason { get; set; }

        public string ObjectType { get; set; }

        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}