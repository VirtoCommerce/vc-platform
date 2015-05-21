using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Zendesk.HelpdeskModule.Web.Services;
using ZendeskApi_v2;
using ZendeskApi_v2.Models.Tickets;

namespace Zendesk.HelpdeskModule.Web.Controllers.Api
{
    [RoutePrefix("api/help")]
    public class ZendeskController: ApiController
    {
        private readonly IHelpdesk _zendeskSettings;
        private ZendeskApi _api;

        public ZendeskController(IHelpdesk zendeskSettings)
        {
            _zendeskSettings = zendeskSettings;
            if (!string.IsNullOrEmpty(_zendeskSettings.AccessToken))
            {
                _api = GetZendeskApi(_zendeskSettings.Subdomain, _zendeskSettings.AccessToken);
            }
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("tickets")]
        public IHttpActionResult GetTickets()
        {
                var tickets = _api.Tickets.GetAllTickets();
                return Ok(tickets);
        }

        [HttpGet]
        [ResponseType(typeof(Ticket[]))]
        [Route("tickets/{status}")]
        public IHttpActionResult GetTickets(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                var tickets = _api.Tickets.GetAllTickets().Tickets.Where(t => t.Status == status).ToArray();
                return Ok(tickets);
            }
            return BadRequest();
        }

        //Get by email
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("requester/{email}")]
        public IHttpActionResult GetRequester(string email)
        {
                var user = _api.Users.SearchByEmail(email);
                return Ok(user);
        }
        
        //Get by status and requester email
        [HttpGet]
        [ResponseType(typeof(Ticket[]))]
        [Route("tickets/{status}/{email}")]
        public IHttpActionResult GetNewTickets(string status, string email)
        {
            if (_api != null)
            {
                var users = _api.Users.SearchByEmail(email);
                if (users.Users != null && users.Users.Any())
                {
                    var user = users.Users[0];
                    if (user.Id.HasValue)
                    {
                        var tickets = _api.Tickets.GetTicketsByUserID(user.Id.Value).Tickets;
                        return string.IsNullOrEmpty(status)
                            ? Ok(tickets.ToArray())
                            : Ok(tickets.Where(t => t.Status == status).ToArray());
                    }
                }
            }
            return Ok();
        }

        //Get by status and requester email
        [HttpGet]
        [ResponseType(typeof(string))]
        [Route("link/{email}")]
        public IHttpActionResult GetLink(string email)
        {
            var retVal = string.Empty;
            if (_api != null)
            {
                var users = _api.Users.SearchByEmail(email);
                if (users.Users != null && users.Users.Any())
                {
                    var user = users.Users[0];
                    if (user.Id.HasValue)
                    {
                        retVal = string.Format("https://{0}.zendesk.com/agent/users/{1}/requested_tickets", _zendeskSettings.Subdomain, user.Id.Value);
                    }
                }
            }

            return Ok(new[] { retVal });
        }

        //Get by status and requester email
        [HttpGet]
        [ResponseType(typeof(string))]
        [Route("dashboard")]
        public IHttpActionResult GetDashboardLink()
        {
            
            return Ok(new[] { string.Format("https://{0}.zendesk.com/agent/dashboard", _zendeskSettings.Subdomain) });
        }

        

        private ZendeskApi GetZendeskApi(string subdomain, string token)
        {
            return new ZendeskApi(string.Format("https://{0}.zendesk.com", subdomain), "", "", token);
        }
    }
}