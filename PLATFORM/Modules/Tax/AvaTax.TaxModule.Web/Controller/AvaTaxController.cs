using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Order.Model;
using Address = AvaTaxCalcREST.Address;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;

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
                var request = GenerateRequest(_taxSettings.CompanyCode, order);
                var getTaxResult = taxSvc.GetTax(request);
                if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                {
                    return BadRequest();
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

        private GetTaxRequest GenerateRequest(string companyCode, CustomerOrder order)
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
            getTaxRequest.Commit = false;
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
            Address address1 = new Address();
            address1.AddressCode = "1";
            address1.Line1 = address.Line1;
            address1.City = address.City;
            address1.Region = address.RegionId;
            address1.PostalCode = address.PostalCode;

            Address[] addresses = { address1 };
            getTaxRequest.Addresses = addresses;

            // Line Data
            // Required Parameters
            var lines = new List<Line>();
            foreach(var li in order.Items.Select((x, i) => new { Value = x, Index = i }))
            {
                var line1 = new Line
                {
                    LineNo = li.Index.ToString(CultureInfo.InvariantCulture),
                    ItemCode = li.Value.ProductId,
                    Qty = li.Value.Quantity,
                    Amount = li.Value.Price,
                    OriginCode = "1",
                    DestinationCode = "1",
                    Description = li.Value.Name,
                    TaxCode = li.Value.ProductId
                };

                // Best Practice Request Parameters

                // Situational Request Parameters
                // line1.CustomerUsageType = "L";
                // line1.Discounted = true;
                // line1.TaxIncluded = true;
                // line1.BusinessIdentificationNo = "234243";
                // line1.TaxOverride = new TaxOverrideDef();
                // line1.TaxOverride.TaxOverrideType = "TaxDate";
                // line1.TaxOverride.Reason = "Adjustment for return";
                // line1.TaxOverride.TaxDate = "2013-07-01";
                // line1.TaxOverride.TaxAmount = "0";

                // Optional Request Parameters
                //line1.Ref1 = "ref123";
                //line1.Ref2 = "ref456";

                lines.Add(line1);
            }
            
            getTaxRequest.Lines = lines.ToArray();
            return getTaxRequest;
        }
    }
}
