using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Models
{
    public class Checkout : Drop
    {
        public IDictionary<string, string> Attributes { get; set; }

        public CustomerAddress BillingAddress { get; set; }

        public bool BuyerAcceptsMarketing { get; set; }

        public string CustomerId { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public decimal DiscountsAmount
        {
            get
            {
                return Discounts != null ? Discounts.Sum(d => d.Amount) : 0M;
            }
        }

        public decimal DiscountsSavings
        {
            get
            {
                return Discounts != null ? Discounts.Sum(d => d.Savings) : 0M;
            }
        }

        public string Email { get; set; }

        public ICollection<GiftCard> GiftCards { get; set; }

        public decimal GiftCardsAmount { get; set; }

        public string Id { get; set; }

        public ICollection<LineItem> LineItems { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public CustomerOrder Order { get; set; }

        public string OrderId
        {
            get
            {
                return Order != null ? Order.Id : null;
            }
        }

        public string OrderName
        {
            get
            {
                return Order != null ? Order.Name : null;
            }
        }

        public int OrderNumber
        {
            get
            {
                return Order != null ? Order.OrderNumber : 0;
            }
        }

        public bool RequiresShipping
        {
            get
            {
                return LineItems != null && LineItems.Any(li => li.RequiresShipping);
            }
        }

        public CustomerAddress ShippingAddress { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public ICollection<ShippingMethod> ShippingMethods { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public decimal ShippingPrice
        {
            get
            {
                decimal price = ShippingMethod != null ? ShippingMethod.Price : 0M;

                var discount = Discounts != null ?
                    Discounts.FirstOrDefault(d => d.Type.Equals("ShippingDiscount", StringComparison.OrdinalIgnoreCase)) : null;

                if (discount != null)
                {
                    price -= discount.Amount;
                }

                return price >= 0 ? price : 0;
            }
        }

        public decimal SubtotalPrice
        {
            get
            {
                return LineItems != null ? LineItems.Sum(li => li.LinePrice) : 0M;
            }
        }

        public ICollection<TaxLine> TaxLines { get; set; }

        public decimal TaxPrice
        {
            get
            {
                return TaxLines != null ? TaxLines.Sum(tl => tl.Price) : 0M;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return SubtotalPrice + ShippingPrice + TaxPrice;
            }
        }

        public string StringifiedShippingPrice { get; set; }

        public string StringifiedTotalPrice { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public string Currency { get; set; }

        public bool GuestLogin { get; set; }
    }
}