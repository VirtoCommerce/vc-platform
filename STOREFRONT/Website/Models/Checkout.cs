using DotLiquid;
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
            this.GiftCards = new List<GiftCard>();
            this.Attributes = new Dictionary<string, string>();
            this.Discounts = new List<Discount>();
            this.LineItems = new List<LineItem>();
            this.PaymentMethods = new List<PaymentMethod>();
            this.ShippingMethods = new List<ShippingMethod>();
            this.TaxLines = new List<TaxLine>();
            this.Transactions = new List<Transaction>();
        }

        [DataMember]
        public string CustomerId { get; set; }

        public List<GiftCard> GiftCards { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public CustomerAddress BillingAddress { get; set; }

        [DataMember]
        public bool BuyerAcceptsMarketing { get; set; }

        public List<Discount> Discounts { get; set; }

        [DataMember]
        public decimal DiscountsAmount { get { return this.Discounts.Sum(d => d.Amount); } }

        [DataMember]
        public decimal DiscountsSavings { get { return this.Discounts.Sum(d => d.Savings); } }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public decimal GiftCardsAmount { get; set; }

        [DataMember]
        public string Id { get; set; }

        public List<LineItem> LineItems { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Note { get; set; }

        public CustomerOrder Order { get; set; }

        [DataMember]
        public string OrderId { get { return this.Order != null ? this.Order.Id : null; } }

        [DataMember]
        public string OrderName { get { return this.Order != null ? this.Order.Name : null; } }

        [DataMember]
        public int OrderNumber { get { return this.Order != null ? this.Order.OrderNumber : 0; } }

        [DataMember]
        public bool RequiresShipping { get; set; }

        [DataMember]
        public CustomerAddress ShippingAddress { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public List<ShippingMethod> ShippingMethods { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        [DataMember]
        public decimal ShippingPrice { get { return this.ShippingMethod != null ? this.ShippingMethod.Price : 0M; } }

        [DataMember]
        public decimal SubtotalPrice { get { return this.LineItems.Sum(li => li.LinePrice); } }

        public List<TaxLine> TaxLines { get; set; }

        [DataMember]
        public decimal TaxPrice { get { return this.TaxLines.Sum(tl => tl.Price); } }

        [DataMember]
        public decimal TotalPrice { get { return this.SubtotalPrice + this.ShippingPrice + this.TaxPrice; } }

        [DataMember]
        public List<Transaction> Transactions { get; set; }

        [DataMember]
        public string Currency { get; set; }
    }
}