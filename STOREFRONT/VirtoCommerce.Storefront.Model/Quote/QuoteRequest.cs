using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteRequest : Entity, IHasLanguage
    {
        public QuoteRequest()
        {
        }

        public QuoteRequest(Currency currency, Language language)
        {
            Addresses = new List<Address>();
            Attachments = new List<Attachment>();
            Items = new List<QuoteItem>();
            TaxDetails = new List<TaxDetail>();
            DynamicProperties = new List<DynamicProperty>();
            Language = language;
            Currency = currency;
            ManualShippingTotal = new Money(currency);
            Totals = new QuoteRequestTotals(currency);
            ManualSubTotal = new Money(currency);
            ManualRelDiscountAmount = new Money(currency);
        }

        public string Number { get; set; }

        public string StoreId { get; set; }

        public string ChannelId { get; set; }

        public bool IsAnonymous { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public CustomerInfo Customer { get; set; }

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

        public Address BillingAddress
        {
            get
            {
                return Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
            }
        }

        public Address ShippingAddress
        {
            get
            {
                return Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);
            }
        }

        public bool RequestShippingQuote
        {
            get
            {
                return ShippingAddress != null;
            }
        }

        public int ItemsCount
        {
            get
            {
                return Items.Count;
            }
        }

        public string Email
        {
            get
            {
                string email = null;

                if (Customer != null)
                {
                    email = Customer.Email;
                }
                if (BillingAddress != null)
                {
                    email = BillingAddress.Email;
                }

                if (string.IsNullOrEmpty(email) && ShippingAddress != null)
                {
                    email = ShippingAddress.Email;
                }

                return email;
            }
        }

        #region IHasLanguage Members
        public Language Language { get; set; }
        #endregion
    }
}