using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using Discount = VirtoCommerce.LiquidThemeEngine.Objects.Discount;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class OrderConverter
    {
        public static Order ToShopifyModel(this CustomerOrder order, IStorefrontUrlBuilder urlBuilder)
        {
            var result = new Order();
            result.InjectFrom<NullableAndEnumValueInjecter>(order);

            result.Cancelled = order.IsCancelled == true;
            result.CancelledAt = order.CancelledDate;
            result.CancelReason = order.CancelReason;
            result.CancelReasonLabel = order.CancelReason;
            result.CreatedAt = order.CreatedDate ?? DateTime.MinValue;
            result.Name = order.Number;
            result.OrderNumber = order.Number;
            result.CustomerUrl = urlBuilder.ToAppAbsolute("/account/order/" + order.Number);

            if (order.Addresses != null)
            {
                var shippingAddress = order.Addresses
                    .FirstOrDefault(a => (a.Type & AddressType.Shipping) == AddressType.Shipping);

                if (shippingAddress != null)
                {
                    result.ShippingAddress = shippingAddress.ToShopifyModel();
                }

                var billingAddress = order.Addresses
                    .FirstOrDefault(a => (a.Type & AddressType.Billing) == AddressType.Billing);

                if (billingAddress != null)
                {
                    result.BillingAddress = billingAddress.ToShopifyModel();
                }
                else if (shippingAddress != null)
                {
                    result.BillingAddress = shippingAddress.ToShopifyModel();
                }

                result.Email = order.Addresses
                    .Where(a => !string.IsNullOrEmpty(a.Email))
                    .Select(a => a.Email)
                    .FirstOrDefault();
            }


            var discountTotal = order.DiscountAmount != null ? order.DiscountAmount.Amount : 0;
            var discounts = new List<Discount>();

            if (order.Discount != null)
            {
                discounts.Add(order.Discount.ToShopifyModel());
            }

            var taxLines = new List<Objects.TaxLine>();

            if (order.InPayments != null)
            {
                var inPayment = order.InPayments
                    .OrderByDescending(p => p.CreatedDate)
                    .FirstOrDefault();

                if (inPayment != null)
                {
                    if (string.IsNullOrEmpty(inPayment.Status))
                    {
                        result.FinancialStatus = inPayment.IsApproved == true ? "Paid" : "Pending";
                        result.FinancialStatusLabel = inPayment.IsApproved == true ? "Paid" : "Pending";
                    }
                    else
                    {
                        result.FinancialStatus = inPayment.Status;
                        result.FinancialStatusLabel = inPayment.Status;
                    }

                    if (inPayment.TaxIncluded == true)
                    {
                        taxLines.Add(new Objects.TaxLine { Title = "Payments tax", Price = inPayment.Tax.Amount * 100 });
                    }
                }
            }

            if (order.Shipments != null)
            {
                result.ShippingMethods = order.Shipments.Select(s => s.ToShopifyModel()).ToArray();
                result.ShippingPrice = result.ShippingMethods.Sum(s => s.Price);

                var orderShipment = order.Shipments.FirstOrDefault();

                if (orderShipment != null)
                {
                    if (string.IsNullOrEmpty(orderShipment.Status))
                    {
                        result.FulfillmentStatus = orderShipment.IsApproved == true ? "Sent" : "Not sent";
                        result.FulfillmentStatusLabel = orderShipment.IsApproved == true ? "Sent" : "Not sent";
                    }
                    else
                    {
                        result.FulfillmentStatus = orderShipment.Status;
                        result.FulfillmentStatusLabel = orderShipment.Status;
                    }

                    if (orderShipment.TaxIncluded == true)
                    {
                        taxLines.Add(new Objects.TaxLine { Title = "Shipping tax", Price = orderShipment.Tax.Amount * 100 });
                    }
                }

                var shipmentsWithTax = order.Shipments
                    .Where(s => s.Tax.Amount > 0)
                    .ToList();

                if (shipmentsWithTax.Count > 0)
                {
                    taxLines.Add(new Objects.TaxLine
                    {
                        Title = "Shipping",
                        Price = shipmentsWithTax.Sum(s => s.Tax.Amount) * 100
                    });
                }

                var shipmentsWithDiscount = order.Shipments
                    .Where(s => s.DiscountAmount.Amount > 0)
                    .ToList();

                if (shipmentsWithDiscount.Any())
                {
                    var shipmentDiscount = shipmentsWithDiscount.Sum(s => s.DiscountAmount.Amount);
                    discountTotal += shipmentDiscount;

                    discounts.Add(new Discount
                    {
                        Type = "ShippingDiscount",
                        Code = "Shipping",
                        Amount = shipmentDiscount * 100,
                        Savings = -shipmentDiscount,
                    });
                }
            }

            if (order.Items != null)
            {
                result.LineItems = order.Items.Select(i => i.ToShopifyModel(urlBuilder)).ToArray();
                result.SubtotalPrice = order.Items.Sum(i => i.BasePrice.Amount * i.Quantity ?? 0) * 100;

                var itemsWithTax = order.Items
                    .Where(i => i.Tax.Amount > 0m)
                    .ToList();

                if (itemsWithTax.Any())
                {
                    taxLines.Add(new Objects.TaxLine
                    {
                        Title = "Line items",
                        Price = itemsWithTax.Sum(i => i.Tax.Amount * 100)
                     });
                }

                var itemsWithDiscount = order.Items
                    .Where(i => i.DiscountAmount.Amount > 0m)
                    .ToList();

                if (itemsWithDiscount.Any())
                {
                    var itemsDiscount = itemsWithDiscount.Sum(i => i.DiscountAmount.Amount);
                    discountTotal += itemsDiscount;

                    discounts.Add(new Discount
                    {
                        Type = "FixedAmountDiscount",
                        Code = "Items",
                        Amount = itemsDiscount * 100,
                        Savings = -itemsDiscount * 100,
                    });
                }
            }

            result.TaxLines = taxLines.ToArray();
            result.TaxPrice = taxLines.Sum(t => t.Price);

            result.Discounts = discounts.ToArray();

            result.TotalPrice = order.Sum.Amount * 100;

            return result;
        }
    }
}
