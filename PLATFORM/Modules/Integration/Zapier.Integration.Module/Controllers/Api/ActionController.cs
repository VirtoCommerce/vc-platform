using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Customer.Model;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Controllers.Api
{
    [RoutePrefix("api/zapier")]
    public class ActionController : ApiController
    {
        private readonly IContactsProvider _contactsProvider;

        public ActionController(IContactsProvider contactsProvider)
        {
            _contactsProvider = contactsProvider;
        }

        [HttpPost]
        [ResponseType(typeof(Contact))]
        [Route("contact")]
        public async Task<IHttpActionResult> CreateContact(Contact contact)
        {
            var ret = await Task.FromResult(_contactsProvider.NewContact(contact));
            if (ret != null)
            {
                return Ok(ret);
            }

            return BadRequest();
        }
    }
}
