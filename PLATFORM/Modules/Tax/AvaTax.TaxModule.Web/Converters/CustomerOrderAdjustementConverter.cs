using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvaTaxCalcREST;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;
using Address = AvaTaxCalcREST.Address;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class CustomerOrderAdjustmentConverter
    {
        public static GetTaxRequest ToAvaTaxAdjustmentRequest(this VirtoCommerce.Domain.Order.Model.CustomerOrder modifiedOrder, string companyCode, Contact contact, VirtoCommerce.Domain.Order.Model.CustomerOrder originalOrder, bool commit = false)
        {
            if (modifiedOrder.Addresses != null && modifiedOrder.Addresses.Any() && originalOrder.Items != null && originalOrder.Items.Any())
            {
                // Document Level Elements
                // Required Request Parameters
                var getTaxRequest = new GetTaxRequest
                {
                    CustomerCode = modifiedOrder.CustomerId,
                    DocDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    CompanyCode = companyCode,
                    Client = "VirtoCommerce,2.x,VirtoCommerce",
                    DocCode = string.Format("{0}.{1}", originalOrder.Number, DateTime.UtcNow.ToString("yy-MM-dd-hh-mm")),
                    DetailLevel = DetailLevel.Tax,
                    Commit = commit,
                    DocType = DocType.ReturnInvoice,
                    TaxOverride = new TaxOverrideDef
                    {
                        TaxOverrideType = "TaxDate",
                        Reason = "Adjustment for return",
                        TaxDate = originalOrder.CreatedDate == DateTime.MinValue
                            ? DateTime.UtcNow.ToString("yyyy-MM-dd")
                            : originalOrder.CreatedDate.ToString("yyyy-MM-dd"),
                        TaxAmount = "0"
                    }
                };

                // Best Practice Request Parameters

                // Situational Request Parameters
                // getTaxRequest.CustomerUsageType = "G";
                // getTaxRequest.ExemptionNo = "12345";
                // getTaxRequest.BusinessIdentificationNo = "234243"; //for VAT tax calculations
                // getTaxRequest.Discount = 50;

                // Optional Request Parameters
                //getTaxRequest.PurchaseOrderNo = order.Number;
                getTaxRequest.ReferenceCode = originalOrder.Number;
                //getTaxRequest.PosLaneCode = "09";
                getTaxRequest.CurrencyCode = modifiedOrder.Currency.ToString();

                //add customer tax exemption code to cart if exists
                getTaxRequest.ExemptionNo = contact.GetDynamicPropertyValue("Tax exempt", string.Empty);

                string destinationAddressIndex = "0";

                // Address Data
                var addresses = new List<Address>();

                foreach (var address in modifiedOrder.Addresses.Select((x, i) => new { Value = x, Index = i }))
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

                getTaxRequest.Lines = originalOrder.Items.Where(i => !modifiedOrder.Items.Any(mli => mli.Id.Equals(i.Id)) || i.Quantity > (modifiedOrder.Items.Single(oi => oi.Id.Equals(i.Id)).Quantity)).Select(li =>
                    new Line
                    {
                        LineNo = li.Id,
                        ItemCode = li.ProductId,
                        Qty = modifiedOrder.Items.Any(mli => mli.Id.Equals(li.Id)) ? Math.Abs(li.Quantity - modifiedOrder.Items.Single(oi => oi.Id.Equals(li.Id)).Quantity) : li.Quantity,
                        Amount = modifiedOrder.Items.Any(mli => mli.Id.Equals(li.Id)) ? -(li.Price * li.Quantity - modifiedOrder.Items.Single(oi => oi.Id.Equals(li.Id)).Price * modifiedOrder.Items.Single(mli => mli.Id.Equals(li.Id)).Quantity) : -li.Price * li.Quantity,
                        OriginCode = destinationAddressIndex, //TODO set origin address (fulfillment?)
                        DestinationCode = destinationAddressIndex,
                        Description = li.Name,
                        TaxCode = li.TaxType
                    }
                    ).ToList();

                return getTaxRequest;
            }
            return null;
        }
    }
}