#region
using System;
using System.Linq;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.Web.Convertors
{
    public static class OrderConverters
    {
        #region Public Methods and Operators
        public static CustomerOrder AsWebModel(this Data.Orders.CustomerOrder customerOrder)
        {
            var ret = new CustomerOrder { Id = customerOrder.Id };

            if (customerOrder.Addresses != null)
            {
                var billingAddress = customerOrder.Addresses.FirstOrDefault(a =>
                    a.AddressType == Data.Orders.AddressType.Billing);

                if (billingAddress != null)
                {
                    ret.BillingAddress = billingAddress.AsWebModel();
                }

                var shippingAddress = customerOrder.Addresses.FirstOrDefault(a =>
                    a.AddressType == Data.Orders.AddressType.Shipping);

                if (shippingAddress != null)
                {
                    ret.ShippingAddress = shippingAddress.AsWebModel();
                }
            }

            ret.Cancelled = customerOrder.IsCancelled;
            ret.CancelledAt = customerOrder.CancelledDate;
            ret.CancelReason = customerOrder.CancelReason;
            ret.CancelReasonLabel = customerOrder.CancelReason;

            ret.CreatedAt = customerOrder.CreatedDate;
            ret.CustomerUrl = new Uri(string.Format("account/order/{0}", customerOrder.Id), UriKind.Relative).ToString();

            if (customerOrder.Discount != null)
            {
                ret.Discounts.Add(customerOrder.Discount.AsWebModel());
            }

            ret.Email = string.Empty; // TODO
            ret.FinancialStatus = string.Empty; // TODO
            ret.FinancialStatusLabel = string.Empty; // TODO
            ret.FulfillmentStatus = string.Empty; // TODO
            ret.FullfillmentStatusLabel = string.Empty; // TODO

            if (customerOrder.Items != null)
            {
                foreach (var lineItem in customerOrder.Items)
                {
                    ret.LineItems.Add(lineItem.AsWebModel());
                }
            }

            ret.Name = customerOrder.Number;
            ret.OrderNumber = 0; // TODO

            if (customerOrder.Shipments != null)
            {
                foreach (var shipment in customerOrder.Shipments)
                {
                    ret.ShippingMethods.Add(shipment.AsWebModel());
                    ret.ShippingPrice += shipment.Sum;
                }
            }

            ret.SubtotalPrice = ret.LineItems.Sum(li => li.Quantity * li.Price);

            if (customerOrder.TaxIncluded)
            {
                ret.TaxLines.Add(new TaxLine { Price = customerOrder.Tax });
                ret.TaxPrice = ret.TaxLines.Sum(tl => tl.Price);
            }

            ret.TotalPrice = customerOrder.Sum;

            return ret;
        }
        public static CustomerAddress AsWebModel(this ApiClient.DataContracts.Orders.Address address)
        {
            var ret = new CustomerAddress
                      {
                          Address1 = address.Line1,
                          Address2 = address.Line2,
                          City = address.City,
                          Company = address.Organization,
                          Country = address.CountryName,
                          CountryCode = address.CountryCode,
                          FirstName = address.FirstName,
                          LastName = address.LastName,
                          Phone = address.Phone,
                          Province = address.RegionName,
                          Zip = address.PostalCode
                      };

            return ret;
        }

        public static Discount AsWebModel(this ApiClient.DataContracts.Orders.Discount discount)
        {
            var ret = new Discount
                      {
                          Amount = discount.DiscountAmount,
                          Code = discount.PromotionId,
                          Id = discount.PromotionId,
                          Savings = -discount.DiscountAmount
                      };

            return ret;
        }

        public static ShippingMethod AsWebModel(this Data.Orders.Shipment shipment)
        {
            var ret = new ShippingMethod
            {
                Price = shipment.Sum,
                Title = shipment.ShipmentMethodCode,
                Handle = shipment.ShipmentMethodCode
            };

            return ret;
        }

        public static LineItem AsWebModel(this ApiClient.DataContracts.Orders.LineItem lineItem)
        {
            var ret = new LineItem
                      {
                          Fulfillment = null,
                          Grams = 0,
                          Handle = null,
                          Id = lineItem.Id,
                          Image = lineItem.ImageUrl,
                          Price = lineItem.Price,
                          Product = null,
                          ProductId = lineItem.ProductId,
                          Quantity = lineItem.Quantity,
                          RequiresShipping = false,
                          Sku = null,
                          Title = lineItem.Name,
                          Url = null,
                          Variant = null,
                          VariantId = null,
                          Vendor = null
                      };

            return ret;
        }
        #endregion
    }
}