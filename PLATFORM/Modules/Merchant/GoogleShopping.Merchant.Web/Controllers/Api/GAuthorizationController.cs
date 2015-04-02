using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using Google.Apis.Services;
using Google.Apis.ShoppingContent.v2;
using VirtoCommerce.Framework.Web.Settings;
using Google.Apis.Auth.OAuth2;

namespace GoogleShopping.MerchantModule.Web.Controllers.Api
{
    [RoutePrefix("g/api/auth")]
    public class GAuthorizationController : ApiController
    {
        private const string _accessTokenPropertyName = "Google.Shopping.Credentials.AccessToken";
        
        private readonly ISettingsManager _settingsManager;

        public GAuthorizationController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("complete")]
        public IHttpActionResult Complete()
        {
            
            return Ok();
        }
    }
}
