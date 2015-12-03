using System;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Order : Drop
    {
        public Address BillingAddress { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string CancelReason { get; set; }
        public string CancelReasonLabel { get; set; }
        public DateTime CreatedAt { get; set; }
        public Customer Customer { get; set; }
        public string CustomerUrl { get; set; }
        public Discount[] Discounts { get; set; }
        public string Email { get; set; }
        public string FinancialStatus { get; set; }
        public string FinancialStatusLabel { get; set; }
        public string FulfillmentStatus { get; set; }
        public string FulfillmentStatusLabel { get; set; }
        public LineItem[] LineItems { get; set; }
        public object Location { get; set; }
        public string OrderStatusUrl { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string OrderNumber { get; set; }
        public Address ShippingAddress { get; set; }
        public ShippingMethod[] ShippingMethods { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal SubtotalPrice { get; set; }
        public TaxLine[] TaxLines { get; set; }
        public decimal TaxPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public object[] Transactions { get; set; }
    }
}
