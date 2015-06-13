using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Order.Model;
using Address = AvaTaxCalcREST.Address;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;
using CartAddressType = VirtoCommerce.Domain.Cart.Model.AddressType;

namespace AvaTax.TaxModule.Web.Controller
{
    [RoutePrefix("api/tax/avatax")]
    public class AvaTaxController : ApiController
    {
        private readonly ITax _taxSettings;
        //private readonly TaxSvc _taxService;

        public AvaTaxController(ITax taxSettings)//, TaxSvc taxService)
        {
            _taxSettings = taxSettings;
            //_taxService = taxService;
        }
        
        [HttpPost]
        [ResponseType(typeof(CustomerOrder))]
        [Route("")]
        public IHttpActionResult Total(CustomerOrder order)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                var taxSvc = new TaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = OrderTaxRequest(_taxSettings.CompanyCode, order);
                var getTaxResult = taxSvc.GetTax(request);
                if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                {
                    var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Details));
                    return BadRequest(error);
                }
                else
                {
                    foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                    {
                        order.Items.ToArray()[Int32.Parse(taxLine.LineNo)].Tax = taxLine.Tax;
                        //foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                        //{
                        //}
                    }
                    order.Tax = getTaxResult.TotalTax;
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok(order);
        }

        [HttpPost]
        [ResponseType(typeof(ShoppingCart))]
        [Route("cart")]
        public IHttpActionResult CartTotal(ShoppingCart cart)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                var taxSvc = new TaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = CartTaxRequest(_taxSettings.CompanyCode, cart);
                var getTaxResult = taxSvc.GetTax(request);
                if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                {
                    var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Details));
                    return BadRequest(error);
                }
                else
                {
                    foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                    {
                        cart.Items.ToArray()[Int32.Parse(taxLine.LineNo)].TaxTotal = taxLine.Tax;
                        //foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                        //{
                        //}
                    }
                    cart.TaxTotal = getTaxResult.TotalTax;
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok(cart);
        }

        private GetTaxRequest OrderTaxRequest(string companyCode, CustomerOrder order, bool commit = false)
        {
            var getTaxRequest = new GetTaxRequest();

            // Document Level Elements
            // Required Request Parameters
            getTaxRequest.CustomerCode = order.CustomerId;
            getTaxRequest.DocDate = order.CreatedDate.ToShortDateString();

            // Best Practice Request Parameters
            getTaxRequest.CompanyCode = companyCode;
            getTaxRequest.Client = "VirtoCommerce";
            getTaxRequest.DocCode = order.Id;
            getTaxRequest.DetailLevel = DetailLevel.Tax;
            getTaxRequest.Commit = commit;
            getTaxRequest.DocType = DocType.SalesInvoice;

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
            //getTaxRequest.CurrencyCode = order.Currency.ToString();

            // Address Data
            var address =
                order.Addresses.First(
                    x => x.AddressType == AddressType.Shipping || x.AddressType == AddressType.Shipping);
            var address1 = new Address
            {
                AddressCode = "1",
                Line1 = address.Line1,
                City = address.City,
                Region = address.RegionId,
                PostalCode = address.PostalCode
            };

            Address[] addresses = { address1 };
            getTaxRequest.Addresses = addresses;

            // Line Data
            // Required Parameters

            getTaxRequest.Lines = order.Items.Select((x, i) => new { Value = x, Index = i }).Select(li => 
                    new Line
                    {
                        LineNo = li.Index.ToString(CultureInfo.InvariantCulture), 
                        ItemCode = li.Value.ProductId, 
                        Qty = li.Value.Quantity,
                        Amount = li.Value.Price, 
                        OriginCode = "1", 
                        DestinationCode = "1", 
                        Description = li.Value.Name, 
                        TaxCode = li.Value.ProductId
                    }
                ).ToArray();
            return getTaxRequest;
        }

        private GetTaxRequest CartTaxRequest(string companyCode, ShoppingCart cart)
        {
            var getTaxRequest = new GetTaxRequest();

            // Document Level Elements
            // Required Request Parameters
            getTaxRequest.CustomerCode = cart.CustomerId;
            getTaxRequest.DocDate = cart.CreatedDate.ToShortDateString();

            // Best Practice Request Parameters
            getTaxRequest.CompanyCode = companyCode;
            getTaxRequest.Client = "VirtoCommerce";
            getTaxRequest.DocCode = cart.Id;
            getTaxRequest.DetailLevel = DetailLevel.Tax;
            getTaxRequest.Commit = false;
            getTaxRequest.DocType = DocType.SalesOrder;

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
            //getTaxRequest.CurrencyCode = order.Currency.ToString();

            // Address Data
            var address =
                cart.Addresses.First(
                    x => x.AddressType == CartAddressType.Shipping || x.AddressType == CartAddressType.Shipping);
            var address1 = new Address
            {
                AddressCode = "1",
                Line1 = address.Line1,
                City = address.City,
                Region = address.RegionId,
                PostalCode = address.PostalCode
            };

            Address[] addresses = { address1 };
            getTaxRequest.Addresses = addresses;

            // Line Data
            // Required Parameters

            getTaxRequest.Lines = cart.Items.Select((x, i) => new { Value = x, Index = i }).Select(li => 
                    new Line
                    {
                        LineNo = li.Index.ToString(CultureInfo.InvariantCulture), 
                        ItemCode = li.Value.ProductId, 
                        Qty = li.Value.Quantity, 
                        Amount = li.Value.ExtendedPrice, 
                        OriginCode = "1", 
                        DestinationCode = "1", 
                        Description = li.Value.Name, 
                        TaxCode = li.Value.ProductId
                    }
                ).ToArray();
            return getTaxRequest;
        }
    }
}
