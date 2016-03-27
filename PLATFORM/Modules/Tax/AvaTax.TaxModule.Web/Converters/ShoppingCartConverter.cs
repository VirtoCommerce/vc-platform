
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;
using Address = AvaTaxCalcREST.Address;
using AddressType = VirtoCommerce.Domain.Commerce.Model.AddressType;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class ShoppingCartConverter
    {
        public static GetTaxRequest ToAvaTaxRequest(
            this VirtoCommerce.Domain.Cart.Model.ShoppingCart cart,
            string companyCode, Member member,
            bool commit = false)
        {
            if (cart.Addresses != null && cart.Addresses.Any() && cart.Items != null && cart.Items.Any())
            {
                var getTaxRequest = new GetTaxRequest
                {
                    CustomerCode = cart.CustomerId,
                    DocDate = cart.CreatedDate.ToString("yyyy-MM-dd"),
                    CompanyCode = companyCode,
                    Client = "VirtoCommerce,2.x,VirtoCommerce",
                    DocCode = cart.Id,
                    DetailLevel = DetailLevel.Tax,
                    Commit = false,
                    DocType = DocType.SalesOrder,
                    CurrencyCode = cart.Currency.ToString()
                };

                // Document Level Elements
                // Required Request Parameters

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
                //getTaxRequest.PurchaseOrderNo = order.Id;
                //getTaxRequest.ReferenceCode = "ref123456";
                //getTaxRequest.PosLaneCode = "09";

                //add customer tax exemption code to cart if exists
                getTaxRequest.ExemptionNo = member.GetDynamicPropertyValue("Tax exempt", string.Empty);

                // Address Data
                string destinationAddressIndex = "0";

                // Address Data
                var addresses = new List<Address>();

                foreach (var address in cart.Addresses.Select((x, i) => new { Value = x, Index = i }))
                {

                    addresses.Add(
                        new Address
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

                getTaxRequest.Addresses = addresses;

                // Line Data
                // Required Parameters

                getTaxRequest.Lines = cart.Items.Select(li =>
                    new Line
                    {
                        LineNo = li.Id,
                        ItemCode = li.ProductId,
                        Qty = li.Quantity,
                        Amount = li.ExtendedPrice,
                        OriginCode = destinationAddressIndex, //TODO set origin address (fulfillment?)
                        DestinationCode = destinationAddressIndex,
                        Description = li.Name,
                        TaxCode = li.TaxType
                    }
                    ).ToList();

                //Add shipments as lines
                if (cart.Shipments != null && cart.Shipments.Any())
                {
                    cart.Shipments.ForEach(sh =>
                    getTaxRequest.Lines.Add(new Line
                    {
                        LineNo = sh.Id ?? sh.ShipmentMethodCode,
                        ItemCode = sh.ShipmentMethodCode,
                        Qty = 1,
                        Amount = sh.ShippingPrice,
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