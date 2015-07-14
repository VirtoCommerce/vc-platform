using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
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
                    {
                        retVal = BadRequest("Tax calculation disabled, enable before testing connection");
                        throw new Exception((retVal as BadRequestErrorMessageResult).Message);
                    }

                    var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                    var result = taxSvc.Ping();
                    if (!result.ResultCode.Equals(SeverityLevel.Success))
                    {
                        retVal =
                            BadRequest(string.Join(Environment.NewLine,
                                result.Messages.Where(ms => ms.Severity == SeverityLevel.Error).Select(
                            m => m.Summary + string.Format(" [{0} - {1}] ", m.RefersTo, m.Details == null ? string.Empty : string.Join(", ", m.Details)))));
                        throw new Exception((retVal as BadRequestErrorMessageResult).Message);
                    }

                    retVal = Ok(result);
                }
                else
                {
                    retVal = BadRequest("AvaTax credentials not provided");
                    throw new Exception((retVal as BadRequestErrorMessageResult).Message);
                }
            })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.TaxPingError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.Ping);

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
                {
                    retVal = BadRequest("AvaTax address validation disabled");
                    throw new Exception((retVal as BadRequestErrorMessageResult).Message);
                }

                if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                    && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                    && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
                {
                    var addressSvc = new JsonAddressSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                    
                    var request = address.ToValidateAddressRequest(_taxSettings.CompanyCode);
                    
                    var validateAddressResult = addressSvc.Validate(request);
                    if (!validateAddressResult.ResultCode.Equals(SeverityLevel.Success))
                    {
                        var error = string.Join(Environment.NewLine,
                            validateAddressResult.Messages.Where(ms => ms.Severity == SeverityLevel.Error).Select(
                            m => m.Summary + string.Format(" [{0} - {1}] ", m.RefersTo, m.Details == null ? string.Empty : string.Join(", ", m.Details))));
                        retVal = BadRequest(error);
                        throw new Exception((retVal as BadRequestErrorMessageResult).Message);
                    }

                    retVal = Ok(validateAddressResult);
                }

                if (!(retVal is OkNegotiatedContentResult<ValidateResult>))
                {
                    retVal = BadRequest("AvaTax credentials not provided");
                    throw new Exception((retVal as BadRequestErrorMessageResult).Message);
                }
            })
                .OnError(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.AddressValidationError)
                .OnSuccess(VirtoCommerceEventSource.Log, VirtoCommerceEventSource.EventCodes.ValidateAddress);

            return retVal;
        }
    }
}
