using System.Web.Http;
using System.Web.Http.Description;
using AvaTax.TaxModule.Web.Services;

namespace AvaTax.TaxModule.Web.Controller
{
    [RoutePrefix("api/tax/avatax")]
    public class AvaTaxController : ApiController
    {
        private readonly ITax _taxSettings;

        public AvaTaxController(ITax taxSettings)
        {
            _taxSettings = taxSettings;
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("total/{amount}")]
        public IHttpActionResult Total(decimal amount)
        {
            if (!string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password))
            {
                //TODO make request. get response.
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
