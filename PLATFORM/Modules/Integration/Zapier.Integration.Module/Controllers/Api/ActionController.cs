using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Controllers.Api
{
    [AllowAnonymous]
    [RoutePrefix("api/zapier")]
    public class ActionController : ApiController
    {
        private readonly IContactsProvider _contactsProvider;

        public ActionController(IContactsProvider contactsProvider)
        {
            _contactsProvider = contactsProvider;
        }
        
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("customer")]
        public async Task<IHttpActionResult> CreateCustomer()
        {
            return Ok(HttpStatusCode.Created);
        }

    }
}