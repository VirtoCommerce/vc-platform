#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;
#endregion

namespace VirtoCommerce.Web.Models
{

    [DataContract]
    public class CustomerOrder : Drop
    {
        #region Fields
        private ICollection<Discount> _discounts;

        private ICollection<LineItem> _lineItems;

        private ICollection<ShippingMethod> _shippingMethods;
        #endregion

        #region Public Properties
        [DataMember]
        public CustomerAddress BillingAddress { get; set; }

        [DataMember]
        public string CancelReason { get; set; }

        [DataMember]
        public string CancelReasonLabel { get; set; }

        [DataMember]
        public bool Cancelled { get; set; }

        [DataMember]
        public DateTime? CancelledAt { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public string CustomerUrl { get; set; }

        [DataMember]
        public ICollection<Discount> Discounts
        {
            get
            {
                return this._discounts ?? (this._discounts = new HashSet<Discount>());
            }
        }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FinancialStatus { get; set; }

        [DataMember]
        public string FinancialStatusLabel { get; set; }

        [DataMember]
        public string FulfillmentStatus { get; set; }

        [DataMember]
        public string FulfillmentStatusLabel { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public ICollection<LineItem> LineItems
        {
            get
            {
                return this._lineItems ?? (this._lineItems = new HashSet<LineItem>());
            }
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int OrderNumber { get; set; }

        [DataMember]
        public CustomerAddress ShippingAddress { get; set; }

        [DataMember]
        public ICollection<ShippingMethod> ShippingMethods
        {
            get
            {
                return this._shippingMethods ?? (this._shippingMethods = new HashSet<ShippingMethod>());
            }
        }

        [DataMember]
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        [DataMember]
        public decimal ShippingPrice
        {
            get
            {
                return ShippingMethods != null ? ShippingMethods.Sum(sm => sm.Price) : 0M;
            }
        }

        [DataMember]
        public decimal SubtotalPrice
        {
            get
            {
                return LineItems != null ? LineItems.Sum(li => li.LinePrice) : 0M;
            }
        }

        [DataMember]
        public ICollection<TaxLine> TaxLines { get; set; }

        [DataMember]
        public decimal TaxPrice
        {
            get
            {
                return TaxLines != null ? TaxLines.Sum(tl => tl.Price) : 0M;
            }
        }

        [DataMember]
        public decimal TotalPrice
        {
            get
            {
                return SubtotalPrice + TaxPrice + ShippingPrice;
            }
        }
        #endregion
    }
}