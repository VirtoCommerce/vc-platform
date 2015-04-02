using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MailChimp.Helper;
using MailChimp.Lists;
using MailChimp.MailingModule.Web.Services;
using VirtoCommerce.Framework.Web.Settings;

namespace MailChimp.MailingModule.Web.Controllers.Api
{
    [RoutePrefix("mc/api/auth")]
    public class MCAuthorizationController : ApiController
    {
        private const string _accessTokenPropertyName = "MailChimp.Mailing.Credentials.AccessToken";
        private const string _dataCenterPropertyName = "MailChimp.Mailing.Credentials.DataCenter";
        private const string _subscribersListIdPropertyName = "MailChimp.Mailing.SubscribersListId";

        readonly Dictionary<string, string> _configuration = new Dictionary<string, string>(){
        {"client_id", "285052451305"}, //TODO replace with production clientId
        {"client_secret", "b4114b54d83aebd3ccb285c968ee06cc"}, //TODO replace with production secret
        {"redirect_uri", "http://127.0.0.1/platformweb/mc/api/auth/complete"}, //TODO replace 127.0.0.1 with correct auth complete link.
        {"authorize_uri", "https://login.mailchimp.com/oauth2/authorize"},
        {"access_token_uri", "https://login.mailchimp.com/oauth2/token"},
        {"base_uri", "https://login.mailchimp.com/oauth2/"}};

        private readonly ISettingsManager _settingsManager;

        public MCAuthorizationController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("authorize")]
        public IHttpActionResult Authorize()
        {
            var mco = new MailChimpOAuth(_configuration);
            var retVal = mco.getLoginURL();
            return Ok();
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("complete")]
        public IHttpActionResult Complete()
        {
            _configuration.Add("code", HttpContext.Current.Request.QueryString["code"]);
            var mco = new MailChimpOAuth(_configuration);
            var accessToken = mco.getSession();
            var restInfo = (mco.getMetaData());
            if (!string.IsNullOrEmpty(accessToken) && restInfo != null)
            {
                _settingsManager.SetValue(_accessTokenPropertyName, accessToken);
                _settingsManager.SetValue(_dataCenterPropertyName, restInfo.dc);
                SetDefaultSubscribersList(accessToken, restInfo.dc);
            }
            return Ok();
        }

        private void SetDefaultSubscribersList(string accessToken, string dataCenter)
        {
            var mc = new MailChimpManager(accessToken, dataCenter);
            var lists = mc.GetLists();
            if (lists.Data.Any())
                _settingsManager.SetValue(_subscribersListIdPropertyName, lists.Data[0].Id);
        }
    }
}
