using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Checkout : Drop
    {
        public Checkout()
        {
            Discounts = new List<Discount>();
        }

        [DataMember]
        public IDictionary<string, string> Attributes { get; set; }

        [DataMember]
        public CustomerAddress BillingAddress { get; set; }

        [DataMember]
        public bool BuyerAcceptsMarketing { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public ICollection<Discount> Discounts { get; set; }

        [DataMember]
        public decimal DiscountsAmount { get; set; }

        [DataMember]
        public decimal DiscountsSavings
        {
            get
            {
                return Discounts.Sum(d => d.Savings);
            }
        }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public ICollection<GiftCard> GiftCards { get; set; }

        [DataMember]
        public string Coupon { get; set; }

        [DataMember]
        public decimal GiftCardsAmount { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public ICollection<LineItem> LineItems { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public CustomerOrder Order { get; set; }

        [DataMember]
        public string OrderId
        {
            get
            {
                return Order != null ? Order.Id : null;
            }
        }

        [DataMember]
        public string OrderName
        {
            get
            {
                return Order != null ? Order.Name : null;
            }
        }

        [DataMember]
        public int OrderNumber
        {
            get
            {
                return Order != null ? Order.OrderNumber : 0;
            }
        }

        [DataMember]
        public bool RequiresShipping
        {
            get
            {
                return LineItems != null && LineItems.Any(li => li.RequiresShipping);
            }
        }

        [DataMember]
        public CustomerAddress ShippingAddress { get; set; }

        [DataMember]
        public ShippingMethod ShippingMethod { get; set; }

        [DataMember]
        public ICollection<ShippingMethod> ShippingMethods { get; set; }

        [DataMember]
        public PaymentMethod PaymentMethod { get; set; }

        [DataMember]
        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        [DataMember]
        public decimal ShippingPrice { get; set; }

        [DataMember]
        public decimal SubtotalPrice { get; set; }

        [DataMember]
        public ICollection<TaxLine> TaxLines { get; set; }

        [DataMember]
        public decimal TaxPrice { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public string StringifiedShippingPrice { get; set; }

        [DataMember]
        public ICollection<Transaction> Transactions { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public bool GuestLogin { get; set; }
    }
}