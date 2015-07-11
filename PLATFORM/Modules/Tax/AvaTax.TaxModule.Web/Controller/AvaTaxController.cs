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
using domainModel = VirtoCommerce.Domain.Commerce.Model;

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
                    return Ok(retVal);

                return BadRequest(string.Join(", ", retVal.Messages.Select(m => m.Summary)));
            }
            
            return BadRequest("AvaTax credentials not provided");
        }
        
        [HttpPost]
        [ResponseType(typeof(bool))]
        [Route("address")]
        public IHttpActionResult ValidateAddress(VirtoCommerce.Domain.Customer.Model.Address address)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode) && _taxSettings.IsEnabled)
            {
                var addressSvc = new JsonAddressSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var request = address.ToValidateAddressRequest(_taxSettings.CompanyCode);
                var validateAddressResult = addressSvc.Validate(request);
                if (!validateAddressResult.ResultCode.Equals(SeverityLevel.Success))
                {
                    var error = string.Join(Environment.NewLine, validateAddressResult.Messages.Select(m => m.Summary));
                    return BadRequest(error);
                }

                return Ok(validateAddressResult);
            }

            return BadRequest("AvaTax credentials not provided");
        }
    }
}
