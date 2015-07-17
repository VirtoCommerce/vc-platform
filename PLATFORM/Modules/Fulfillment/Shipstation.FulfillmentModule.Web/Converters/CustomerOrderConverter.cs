using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Shipstation.FulfillmentModule.Web.Models.Order;
using System;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;

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
                    OrderDate = String.Format("{0:MM'/'dd'/'yyyy  HH:mm:ss tt}", order.CreatedDate),
                    LastModified = String.Format("{0:MM'/'dd'/'yyyy  HH:mm:ss tt}", order.ModifiedDate),
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
                            WeightSpecified = order.Items.Single(i => i.ProductId == shi.ProductId).Weight == null,
                            WeightUnits = order.Items.Single(i => i.ProductId == shi.ProductId).WeightUnit,

                        };

                        items.Add(item);
                    });

                    retVal.Items = items.ToArray();

                    var billTo = new OrdersOrderCustomerBillTo
                    {
                        Company = sh.DeliveryAddress.Organization,
                        Name = sh.DeliveryAddress.FirstName + sh.DeliveryAddress.LastName,
                        Phone = sh.DeliveryAddress.Phone
                    };

                    var shipTo = new OrdersOrderCustomerShipTo
                    {
                        Company = sh.DeliveryAddress.Organization,
                        Name = sh.DeliveryAddress.FirstName + sh.DeliveryAddress.LastName,
                        Phone = sh.DeliveryAddress.Phone,
                        Address1 = sh.DeliveryAddress.Line1,
                        City = sh.DeliveryAddress.City,
                        PostalCode = sh.DeliveryAddress.PostalCode,
                        Country = sh.DeliveryAddress.CountryCode,
                        State = sh.DeliveryAddress.RegionName
                    };

                    var customer = new OrdersOrderCustomer
                    {
                        CustomerCode = order.CustomerId,
                        BillTo = billTo,
                        ShipTo = shipTo
                    };

                    retVal.Customer = customer;
                });

                return retVal;
            }

            return null;
        }
    }
}