using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvaTaxCalcREST;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class CustomerOrderConverter
    {
        public static GetTaxRequest ToAvaTaxRequest(this VirtoCommerce.Domain.Order.Model.CustomerOrder order, string companyCode, bool commit = false)
        {
            // Document Level Elements
            // Required Request Parameters
            var getTaxRequest = new GetTaxRequest
            {
                CustomerCode = order.CustomerId,
                DocDate = order.CreatedDate.ToShortDateString(),
                CompanyCode = companyCode,
                Client = "VirtoCommerce",
                DocCode = order.Id,
                DetailLevel = DetailLevel.Tax,
                Commit = commit,
                DocType = DocType.SalesInvoice
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
            getTaxRequest.PurchaseOrderNo = order.Id;
            //getTaxRequest.ReferenceCode = "ref123456";
            //getTaxRequest.PosLaneCode = "09";
            //getTaxRequest.CurrencyCode = order.Currency.ToString();

            string destinationAddressIndex = "0";

            // Address Data
             var addresses = new List<Address>();

            foreach(var address in order.Addresses.Select((x, i) => new { Value = x, Index = i }))
            {
                addresses.Add(new Address
                {
                    AddressCode = address.Index.ToString(),
                    Line1 = address.Value.Line1,
                    City = address.Value.City,
                    Region = address.Value.RegionId,
                    PostalCode = address.Value.PostalCode
                });

                if (address.Value.AddressType == AddressType.Shipping
                    || address.Value.AddressType == AddressType.Shipping)
                    destinationAddressIndex = address.Index.ToString();
            }

            getTaxRequest.Addresses = addresses.ToArray();
            
            // Line Data
            // Required Parameters

            getTaxRequest.Lines = order.Items.Select((x, i) => new { Value = x, Index = i }).Select(li =>
                new Line
                {
                    LineNo = li.Index.ToString(CultureInfo.InvariantCulture),
                    ItemCode = li.Value.ProductId,
                    Qty = li.Value.Quantity,
                    Amount = li.Value.Price,
                    OriginCode = destinationAddressIndex, //TODO set origin address (fulfillment?)
                    DestinationCode = destinationAddressIndex,
                    Description = li.Value.Name, 
                    TaxCode = li.Value.ProductId
                }
                ).ToArray();

            return getTaxRequest;
        }
    }
}