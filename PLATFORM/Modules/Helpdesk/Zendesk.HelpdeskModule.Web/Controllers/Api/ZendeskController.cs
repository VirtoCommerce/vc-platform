using System.Web.Http;
using System.Web.Http.Description;
using Zendesk.HelpdeskModule.Web.Services;
using ZendeskApi_v2;

namespace Zendesk.HelpdeskModule.Web.Controllers.Api
{
    [RoutePrefix("api/help")]
    public class ZendeskController: ApiController
    {
        private readonly IHelpdesk _zendeskSettings;

        public ZendeskController(IHelpdesk zendeskSettings)
        {
            _zendeskSettings = zendeskSettings;
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("tickets")]
        public IHttpActionResult GetTickets()
        {
            if (!string.IsNullOrEmpty(_zendeskSettings.AccessToken))
            {
                var api = new ZendeskApi("https://virtowayhelp.zendesk.com", "", "", _zendeskSettings.AccessToken);
                var tickets = api.Tickets.GetAllTickets();
                return Ok(tickets);
            }
            
            return BadRequest();
        }
    }
}