using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using webModel = VirtoCommerce.Platform.Web.Model.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/settings")]
    public class UserSettingsController : ApiController
    {
        private static object _lock = new object();
        private readonly ISettingsManager _settingsManager;
        private readonly ISecurityService _securityService;

        public UserSettingsController(ISecurityService securityService, ISettingsManager settingsManager)
        {
            _securityService = securityService;
            _settingsManager = settingsManager;
        }

        [HttpGet]
        [Route("currentuser/{name}")]
        [ResponseType(typeof(webModel.Setting))]
        public IHttpActionResult GetSetting(string name)
        {
            var retVal = GetCurrentUser().Settings.GetSetting(name);
            if (retVal != null)
            {
                return Ok(retVal);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("currentuser/{name}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateSetting(webModel.Setting setting)
        {
            var currentUser = GetCurrentUser();
            var userSetting = currentUser.Settings.GetSetting(setting.Name);
            if (userSetting == null) return NotFound();
            userSetting.InjectFrom(setting);
            lock (_lock)
            {
                _settingsManager.SaveEntitySettingsValues(currentUser);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private ApplicationUserExtended GetCurrentUser()
        {
            return _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full).Result;
        }
    }
}