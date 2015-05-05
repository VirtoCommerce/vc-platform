using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using VirtoCommerce.Platform.Core.Settings;
using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web.Controllers.Api
{
    [RoutePrefix("api/help")]
    public class ZDAuthorizationController: ApiController
    {

        private const string _accessTokenPropertyName = "Zendesk.Helpdesk.Credentials.AccessToken";
        private const string _subdomainPropertyName = "Zendesk.Helpdesk.Subdomain";
        private const string _state = "vc20";

        private bool isSubdomainSet;

        private readonly Dictionary<string, string> _configuration = new Dictionary<string, string>()
        {
            { "client_id", "virtocommerce" }, //TODO replace with production clientId
            { "client_secret", "cca767999eb4bfd31d32a7a5a66609f7668b9b794cf3a35f5b6f74ce4965f1f8" }, //TODO replace with production secret
            { "redirect_uri", "http://127.0.0.1/platformweb/api/help/complete" }, //TODO replace 127.0.0.1 with correct auth complete link.
            { "authorize_uri", "https://{0}.zendesk.com/oauth/authorizations/new" },
            {"scope", "read write"},
            {"state", _state},
            { "access_token_uri", "https://{0}.zendesk.com/oauth/tokens" },
        };

        private readonly ISettingsManager _settingsManager;

        public ZDAuthorizationController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            var subdomain = _settingsManager.GetValue(_subdomainPropertyName, string.Empty);
            if (!string.IsNullOrEmpty(subdomain))
            {
                _configuration["authorize_uri"] = string.Format(_configuration["authorize_uri"], subdomain);
                _configuration["access_token_uri"] = string.Format(_configuration["access_token_uri"], subdomain);
                isSubdomainSet = true;
            }
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("authorize")]
        public IHttpActionResult Authorize()
        {
            var retVal = string.Empty;
            if (isSubdomainSet)
            {
                var zdo = new ZendeskOAuth(_configuration);
                retVal = zdo.getLoginURL();
            }
            return Ok(new[] { retVal });
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("complete")]
        public IHttpActionResult Complete()
        {
            var state = HttpContext.Current.Request.QueryString["state"];
            if (state.Equals(_state))
            {
                _configuration.Add("code", HttpContext.Current.Request.QueryString["code"]);
                _configuration["scope"] = "read";
                var zdo = new ZendeskOAuth(_configuration);
                var accessToken = zdo.getSession();
                
                if (!string.IsNullOrEmpty(accessToken))
                {
                    _settingsManager.SetValue(_accessTokenPropertyName, accessToken);
                }
                return Ok();
            }
            return new BadRequestResult(this);
        }
    }
}