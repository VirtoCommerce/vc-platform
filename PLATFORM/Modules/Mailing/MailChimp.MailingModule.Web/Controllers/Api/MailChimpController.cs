using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MailChimp.Helper;
using MailChimp.Lists;
using MailChimp.MailingModule.Web.Services;
using VirtoCommerce.Framework.Web.Settings;

namespace MailChimp.MailingModule.Web.Controllers.Api
{
    [RoutePrefix("api/mailing")]
    public class MailChimpController : ApiController
    {
        Dictionary<string, string> _configuration = new Dictionary<string, string>(){
        {"client_id", "285052451305"}, //TODO replace with production clientId
        {"client_secret", "b4114b54d83aebd3ccb285c968ee06cc"}, //TODO replace with production secret
        {"redirect_uri", "http://127.0.0.1/platformweb/api/mailing/complete"}, //TODO replace 127.0.0.1 with correct auth complete link.
        {"authorize_uri", "https://login.mailchimp.com/oauth2/authorize"},
        {"access_token_uri", "https://login.mailchimp.com/oauth2/token"},
        {"base_uri", "https://login.mailchimp.com/oauth2/"}};

        private string _mailChimpApiKey;
        private string _mailChimpListId;
        private readonly ISettingsManager _settingsManager;
        private readonly IMailing _mailing;

        public MailChimpController(ISettingsManager settingsManager, IMailing mailing, string mailChimpApiKey, string mailChimpListId)
        {

            if (mailing == null)
                throw new ArgumentNullException("mailing");

            _mailing = mailing;
            _mailChimpApiKey = mailChimpApiKey;
            _mailChimpListId = mailChimpListId;
            _settingsManager = settingsManager;
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("authorize")]
        public IHttpActionResult Authorize()
        {
            MailChimpOAuth mco = new MailChimpOAuth(_configuration);
            var retVal = mco.getLoginURL();
            return Ok();
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("complete")]
        public IHttpActionResult Complete()
        {
            _configuration.Add("code", HttpContext.Current.Request.QueryString["code"]);
            MailChimpOAuth mco = new MailChimpOAuth(_configuration);
            var session = mco.getSession();
            var rest_info = (mco.getMetaData());
            var api_key = session + "-" + rest_info.dc;
            if (!string.IsNullOrEmpty(api_key))
            {
                _mailChimpApiKey = api_key;
                _settingsManager.SetValue("MailChimp.Mailing.Credentials.ApiKey", api_key);
            }
            return Ok();
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("subscribe/{subscribeEmail}")]
        public IHttpActionResult Subscribe(string subscribeEmail)
        {
            _mailChimpApiKey = _settingsManager.GetValue("MailChimp.Mailing.Credentials.ApiKey", string.Empty);
            if (!string.IsNullOrEmpty(_mailChimpApiKey) && subscribeEmail.Contains("@") && subscribeEmail.Contains("."))
            {
                var mc = new MailChimpManager(_mailChimpApiKey);

                //  Create the email parameter
                var email = new EmailParameter()
                {
                    Email = subscribeEmail,
                };

                var results = mc.Subscribe(_mailChimpListId, email);
                if (string.IsNullOrEmpty(results.EUId))
                    return BadRequest();
            }
            return Ok();
        }
    }
}
