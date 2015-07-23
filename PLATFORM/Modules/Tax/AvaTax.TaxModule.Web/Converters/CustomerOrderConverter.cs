using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;
using Address = AvaTaxCalcREST.Address;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class CustomerOrderConverter
    {
        public static GetTaxRequest ToAvaTaxRequest(this VirtoCommerce.Domain.Order.Model.CustomerOrder order, string companyCode, Contact contact, bool commit = false)
        {
            if (order.Addresses != null && order.Addresses.Any() && order.Items != null && order.Items.Any())
            {
                // Document Level Elements
                // Required Request Parameters
                var getTaxRequest = new GetTaxRequest
                {
                    CustomerCode = order.CustomerId,
                    DocDate =
                        order.CreatedDate == DateTime.MinValue
                            ? DateTime.UtcNow.ToString("yyyy-MM-dd")
                            : order.CreatedDate.ToString("yyyy-MM-dd"),
                    CompanyCode = companyCode,
                    Client = "VirtoCommerce,2.x,VirtoCommerce",
                    DetailLevel = DetailLevel.Tax,
                    Commit = commit,
                    DocType = DocType.SalesInvoice,
                    DocCode = order.Number,
                    CurrencyCode = order.Currency.ToString()
                };

                // Best Practice Request Parameters

                // Situational Request Parameters
                // getTaxRequest.CustomerUsageType = "G";
                // getTaxRequest.ExemptionNo = "12345";
                // getTaxRequest.BusinessIdentificationNo = "234243";
                // getTaxRequest.Discount = 50;
                // getTaxRequest.TaxOverride = new TaxOverrideDef();
                // getTaxRequest.TaxOverride.TaxOverrideType = "TaxDate";
                // getTaxRequest.TaxOverride.Reason = "Adjustment for return";
                // getTaxRequest.TaxOverride.TaxDate = "2013-07-01";
                // getTaxRequest.TaxOverride.TaxAmount = "0";

                // Optional Request Parameters
                //getTaxRequest.PurchaseOrderNo = order.Number;
                //getTaxRequest.ReferenceCode = "ref123456";
                //getTaxRequest.PosLaneCode = "09";

                //add customer tax exemption code to cart if exists
                getTaxRequest.ExemptionNo = contact.GetDynamicPropertyValue("Tax exempt", string.Empty);

                string destinationAddressIndex = "0";

                // Address Data
                var addresses = new List<Address>();

                foreach (var address in order.Addresses.Select((x, i) => new { Value = x, Index = i }))
                {
                    addresses.Add(new Address
                    {
                        AddressCode = address.Index.ToString(CultureInfo.InvariantCulture),
                        Line1 = address.Value.Line1,
                        City = address.Value.City,
                        Region = address.Value.RegionName ?? address.Value.RegionId,
                        PostalCode = address.Value.PostalCode,
                        Country = address.Value.CountryName
                    });

                    if (address.Value.AddressType == AddressType.Shipping
                        || address.Value.AddressType == AddressType.Shipping)
                        destinationAddressIndex = address.Index.ToString(CultureInfo.InvariantCulture);
                }

                getTaxRequest.Addresses = addresses.ToArray();

                // Line Data
                // Required Parameters

                getTaxRequest.Lines = order.Items.Select(li =>
                    new Line
                    {
                        LineNo = li.Id,
                        ItemCode = li.ProductId,
                        Qty = li.Quantity,
                        Amount = li.Price * li.Quantity,
                        OriginCode = destinationAddressIndex, //TODO set origin address (fulfillment?)
                        DestinationCode = destinationAddressIndex,
                        Description = li.Name,
                        TaxCode = li.TaxType
                    }
                    ).ToList();

                //Add shipments as lines
                if (order.Shipments != null && order.Shipments.Any())
                {
                    order.Shipments.ForEach(sh =>
                    getTaxRequest.Lines.Add(new Line
                    {
                        LineNo = sh.Id ?? sh.ShipmentMethodCode,
                        ItemCode = sh.ShipmentMethodCode,
                        Qty = 1,
                        Amount = sh.Sum,
                        OriginCode = destinationAddressIndex, //TODO set origin address (fulfillment?)
                        DestinationCode = destinationAddressIndex,
                        Description = sh.ShipmentMethodCode,
                        TaxCode = sh.TaxType ?? "FR"
                    })
                    );
                }

                return getTaxRequest;
            }
            return null;
        }
    }
}