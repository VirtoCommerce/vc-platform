using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Logging;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
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
            IHttpActionResult retVal = BadRequest();
            SlabInvoker<VirtoCommerceEventSource.TaxRequestContext>.Execute(slab =>
            {
                if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                    && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                    && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
                {
                    if (!_taxSettings.IsEnabled)
                        retVal = BadRequest("Tax calculation disabled, enable before testing connection");

                    var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                    var result = taxSvc.Ping();
                    if (result.ResultCode.Equals(SeverityLevel.Success))
                        retVal = Ok(result);

                    retVal =
                        BadRequest(string.Join(", ",
                            result.Messages.Select(
                                m => m.Summary + "-" + Environment.NewLine + string.Join(Environment.NewLine, m.Details))));
                }

                retVal = BadRequest("AvaTax credentials not provided");
            })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.TaxPingError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.GetTaxRequestTime);

            return retVal;
        }

        [HttpPost]
        [ResponseType(typeof(bool))]
        [Route("address")]
        public IHttpActionResult ValidateAddress(VirtoCommerce.Domain.Customer.Model.Address address)
        {
            IHttpActionResult retVal = BadRequest();
            SlabInvoker<VirtoCommerceEventSource.TaxRequestContext>.Execute(slab =>
            {
                if (!_taxSettings.IsValidateAddress)
                    retVal = BadRequest("AvaTax address validation disabled");

                if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                    && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                    && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
                {
                    var addressSvc = new JsonAddressSvc(_taxSettings.Username,
                        _taxSettings.Password,
                        _taxSettings.ServiceUrl);
                    var request = address.ToValidateAddressRequest(_taxSettings.CompanyCode);
                    var validateAddressResult = addressSvc.Validate(request);
                    if (!validateAddressResult.ResultCode.Equals(SeverityLevel.Success))
                    {
                        var error = string.Join(Environment.NewLine,
                            validateAddressResult.Messages.Select(
                                m => m.Summary + "-" + Environment.NewLine + string.Join(Environment.NewLine, m.Details)));
                        retVal = BadRequest(error);
                    }

                    retVal = Ok(validateAddressResult);
                }

                retVal = BadRequest("AvaTax credentials not provided");
            })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.TaxCalculationError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.GetTaxRequestTime);

            return retVal;
        }
    }
}
