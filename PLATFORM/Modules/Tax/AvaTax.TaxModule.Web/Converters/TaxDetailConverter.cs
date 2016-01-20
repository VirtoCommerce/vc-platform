using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvaTaxCalcREST;
using VirtoCommerce.Platform.Core.DynamicProperties;
using Address = AvaTaxCalcREST.Address;
using VirtoCommerce.Domain.Tax.Model;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class TaxRequestConverter
    {
        public static GetTaxRequest ToAvaTaxRequest(this TaxEvaluationContext evalContext, string companyCode, bool commit = false)
        {
            if (evalContext.Address != null && evalContext.Lines != null && evalContext.Lines.Any())
            {
                // Document Level Elements
                // Required Request Parameters
                var getTaxRequest = new GetTaxRequest
                {
                    CustomerCode = evalContext.Customer.Id,
                    DocDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    CompanyCode = companyCode,
                    Client = "VirtoCommerce,2.x,VirtoCommerce",
                    DetailLevel = DetailLevel.Tax,
                    Commit = commit,
                    DocType = DocType.SalesInvoice,
                    DocCode = evalContext.Id,
                    CurrencyCode = evalContext.Currency.ToString()
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
                getTaxRequest.ExemptionNo = evalContext.Customer.GetDynamicPropertyValue("Tax exempt", string.Empty);

                string destinationAddressIndex = "0";

                // Address Data
                var addresses = new List<Address>{
                    new Address
                    {
                        AddressCode = evalContext.Address.AddressType.ToString(),
                        Line1 = evalContext.Address.Line1,
                        City = evalContext.Address.City,
                        Region = evalContext.Address.RegionName ?? evalContext.Address.RegionId,
                        PostalCode = evalContext.Address.PostalCode,
                        Country = evalContext.Address.CountryName
                    } };
                
                getTaxRequest.Addresses = addresses.ToArray();

                // Line Data
                // Required Parameters

                getTaxRequest.Lines = evalContext.Lines.Select(li =>
                    new Line
                    {
                        LineNo = li.Id,
                        ItemCode = li.Code,
                        Qty = li.Amount,
                        Amount = li.Price * li.Amount,
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