using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Shipstation.FulfillmentModule.Web.Models.Order;
using System;
using System.Linq;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;

namespace Shipstation.FulfillmentModule.Web.Converters
{
    public static class CustomerOrderConverter
    {
        public static OrdersOrder ToShipstationOrder(this VirtoCommerce.Domain.Order.Model.CustomerOrder order)
        {
            if (order.Shipments != null && order.Shipments.Any())
            {
                var retVal = new OrdersOrder
                {
                    OrderNumber = order.Number,
                    OrderID = order.Id,
                    OrderStatus = order.Status,
                    OrderDate = String.Format("{0:MM'/'dd'/'yyyy HH:mm}", order.CreatedDate),
                    LastModified = String.Format("{0:MM'/'dd'/'yyyy HH:mm}", order.ModifiedDate),
                    OrderTotal = (float) order.Sum,
                    ShippingAmount = (float) order.Shipments.Sum(sh => sh.Sum),
                    TaxAmount = (float) order.Tax,
                    ShippingMethod = order.Shipments.First().ShipmentMethodCode,
                    ShippingAmountSpecified = true,
                    PaymentMethod = order.InPayments.First().GatewayCode
                };
                var items = new List<OrdersOrderItem>();
                order.Shipments.Where(s => s.Items != null && s.Items.Any()).ForEach(sh =>
                {
                    sh.Items.ForEach(shi =>
                    {
                        var item = new OrdersOrderItem
                        {
                            SKU = shi.ProductId,
                            ImageUrl = shi.ImageUrl,
                            LineItemID = shi.LineItemId,
                            Name = shi.Name,
                            Quantity = (sbyte)shi.Quantity,
                            UnitPrice = (float)order.Items.Single(i => i.ProductId == shi.ProductId).Price,
                            Weight = (float)(order.Items.Single(i => i.ProductId == shi.ProductId).Weight ?? 0),
                            WeightSpecified = order.Items.Single(i => i.ProductId == shi.ProductId).Weight != null,
                            WeightUnits = order.Items.Single(i => i.ProductId == shi.ProductId).WeightUnit,

                        };

                        items.Add(item);
                    });

                    retVal.Items = items.ToArray();
                });

                var customer = new OrdersOrderCustomer
                {
                    CustomerCode = order.CustomerId
                };

                var billAddress =
                    order.Addresses.FirstOrDefault(
                        a => a.AddressType == AddressType.Billing || a.AddressType == AddressType.BillingAndShipping);

                if (billAddress != null)
                {
                    var billTo = new OrdersOrderCustomerBillTo
                    {
                        Company = billAddress.Organization,
                        Name = billAddress.FirstName + " " + billAddress.LastName,
                        Phone = billAddress.Phone
                    };

                    customer.BillTo = billTo;
                }

                var shipAddress =
                    order.Addresses.FirstOrDefault(
                        a => a.AddressType == AddressType.Shipping || a.AddressType == AddressType.BillingAndShipping);

                if (shipAddress != null)
                {

                    var shipTo = new OrdersOrderCustomerShipTo
                    {
                        Company = shipAddress.Organization,
                        Name = shipAddress.FirstName + " " + shipAddress.LastName,
                        Phone = shipAddress.Phone,
                        Address1 = shipAddress.Line1,
                        City = shipAddress.City,
                        PostalCode = shipAddress.PostalCode,
                        Country = shipAddress.CountryCode,
                        State = shipAddress.RegionId ?? shipAddress.RegionName
                    };

                    customer.ShipTo = shipTo;
                }

                retVal.Customer = customer;

                return retVal;
            }

            return null;
        }
    }
}