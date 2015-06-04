using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Customer.Model;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Controllers.Api
{
    [AllowAnonymous]
    [RoutePrefix("api/zapier")]
    public class PollingController : ApiController
    {
        private readonly IContactsProvider _contactsProvider;
        private readonly IOrdersProvider _ordersProvider;

        public PollingController(IContactsProvider contactsProvider, IOrdersProvider ordersProvider)
        {
            _contactsProvider = contactsProvider;
            _ordersProvider = ordersProvider;
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok();
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("payments")]
        public IHttpActionResult NewPayments()
        {
            return Ok(_contactsProvider.GetNewContacts());
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("contacts")]
        public IHttpActionResult NewContacts()
        {
            return Ok(_contactsProvider.GetNewContacts());
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("orders")]
        public IHttpActionResult NewOrders()
        {
            return Ok(_ordersProvider.GetNewOrders());
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("hooks")]
        public IHttpActionResult PostWebHooks()
        {
            var result = HttpStatusCode.Created;
            return Ok(result);
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("hooks/{id}")]
        public async Task<IHttpActionResult> DeleteWebHook(string id)
        {
            await Task.FromResult<object>(null);
            return Ok();
        }
    }
}
