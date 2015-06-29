using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Order.Model;
using CartAddressType = VirtoCommerce.Domain.Cart.Model.AddressType;

namespace AvaTax.TaxModule.Web.Controller
{
    [RoutePrefix("api/tax/avatax")]
    public class AvaTaxController : ApiController
    {
        private readonly ITaxSettings _taxSettings;

        public AvaTaxController(ITaxSettings taxSettings)
        {
            _taxSettings = taxSettings;
        }
        
        [HttpPost]
        [ResponseType(typeof(CustomerOrder))]
        [Route("")]
        public IHttpActionResult Total(CustomerOrder order)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode) && _taxSettings.IsEnabled)
            {
                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = order.ToAvaTaxRequest(_taxSettings.CompanyCode);
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

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("ping")]
        public IHttpActionResult TestConnection()
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                if (!_taxSettings.IsEnabled)
                    return BadRequest("Tax calculation disabled, enable before testing connection");

                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var retVal = taxSvc.Ping();
                if (retVal.ResultCode.Equals(SeverityLevel.Success))
                    return Ok(new[] {retVal});

                return BadRequest(string.Join(", ", retVal.Messages.Select(m => m.Summary)));
            }
            
            return BadRequest("AvaTax credentials not provided");
        }

        [HttpPost]
        [ResponseType(typeof(ShoppingCart))]
        [Route("cart")]
        public IHttpActionResult CartTotal(ShoppingCart cart)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode) && _taxSettings.IsEnabled)
            {
                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = cart.ToAvaTaxRequest(_taxSettings.CompanyCode);
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

        [HttpPost]
        [ResponseType(typeof(bool))]
        [Route("cancel")]
        public IHttpActionResult CancelTax(CustomerOrder order)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode) && _taxSettings.IsEnabled)
            {
                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = order.ToAvaTaxCancelRequest(_taxSettings.CompanyCode, CancelCode.DocVoided);
                var cancelTaxResult = taxSvc.CancelTax(request);
                if (!cancelTaxResult.ResultCode.Equals(SeverityLevel.Success))
                {
                    var error = string.Join(Environment.NewLine, cancelTaxResult.Messages.Select(m => m.Details));
                    return BadRequest(error);
                }

                return Ok(cancelTaxResult);
            }
            
            return BadRequest();
        }
    }
}
