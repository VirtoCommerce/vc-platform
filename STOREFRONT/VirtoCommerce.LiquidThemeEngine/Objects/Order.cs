using DotLiquid;
using System;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents the order object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/order
    /// </remarks>
    [DataContract]
    public class Order : Drop
    {
        /// <summary>
        /// Returns the billing address of the order.
        /// </summary>
        [DataMember]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Returns true if an order is canceled, returns false if it is not.
        /// </summary>
        [DataMember]
        public bool Cancelled { get; set; }

        /// <summary>
        /// Returns the timestamp of when an order was canceled. Use the date filter to format the timestamp.
        /// </summary>
        [DataMember]
        public DateTime? CancelledAt { get; set; }

        /// <summary>
        /// Returns one of the following cancellation reasons, if an order was canceled:
        /// items unavailable, fraudulent order, customer changed/cancelled order, other
        /// </summary>
        [DataMember]
        public string CancelReason { get; set; }

        /// <summary>
        /// Returns the translated output of an order's order.cancel_reason.
        /// </summary>
        [DataMember]
        public string CancelReasonLabel { get; set; }

        /// <summary>
        /// Returns the timestamp of when an order was created. Use the date filter to format the timestamp.
        /// </summary>
        [DataMember]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Returns the customer associated with the order.
        /// </summary>
        [DataMember]
        public Customer Customer { get; set; }

        /// <summary>
        /// Returns the URL of the customer's account page.
        /// </summary>
        [DataMember]
        public string CustomerUrl { get; set; }

        /// <summary>
        /// Returns an array of discounts for an order.
        /// </summary>
        [DataMember]
        public Discount[] Discounts { get; set; }

        /// <summary>
        /// Returns the email address associated with an order.
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Returns the financial status of an order. The possible values are:
        /// pending, authorized, paid, partially_paid, refunded, partially_refunded, voided
        /// </summary>
        [DataMember]
        public string FinancialStatus { get; set; }

        /// <summary>
        /// Returns the translated output of an order's financial_status.
        /// </summary>
        [DataMember]
        public string FinancialStatusLabel { get; set; }

        /// <summary>
        /// Returns the fulfillment status of an order.
        /// </summary>
        [DataMember]
        public string FulfillmentStatus { get; set; }

        /// <summary>
        /// Returns the translated output of an order's fulfillment_status.
        /// </summary>
        [DataMember]
        public string FulfillmentStatusLabel { get; set; }

        /// <summary>
        /// Returns an array of line items from the order.
        /// </summary>
        [DataMember]
        public LineItem[] LineItems { get; set; }

        /// <summary>
        /// Returns the physical location of the order.
        /// </summary>
        [DataMember]
        public object Location { get; set; }

        /// <summary>
        /// Returns the link to the order status page for this order.
        /// </summary>
        [DataMember]
        public string OrderStatusUrl { get; set; }

        /// <summary>
        /// Returns the name of the order.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Returns the note associated with a customer order.
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// Returns the integer representation of the order name.
        /// </summary>
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Returns the shipping address of the order.
        /// </summary>
        [DataMember]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Returns an array of shipping_method variables from the order.
        /// </summary>
        [DataMember]
        public ShippingMethod[] ShippingMethods { get; set; }

        /// <summary>
        /// Returns the shipping price of an order. Use a money filter to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal ShippingPrice { get; set; }

        /// <summary>
        /// Returns the subtotal price of an order. Use a money filter to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal SubtotalPrice { get; set; }

        /// <summary>
        /// Returns an array of tax_line variables for an order.
        /// </summary>
        [DataMember]
        public TaxLine[] TaxLines { get; set; }

        /// <summary>
        /// Returns the order's tax price. Use a money filter to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal TaxPrice { get; set; }

        /// <summary>
        /// Returns the total price of an order. Use a money filter to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Returns an array of transactions from the order.
        /// </summary>
        public object[] Transactions { get; set; }
    }
}