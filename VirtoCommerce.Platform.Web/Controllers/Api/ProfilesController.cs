using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Profiles;
using VirtoCommerce.Platform.Web.Converters.Settings;
using webModel = VirtoCommerce.Platform.Web.Model.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/profiles")]
    public class ProfilesController : ApiController
    {
        private static object _lock = new object();
        private readonly ISettingsManager _settingsManager;
        private readonly ISecurityService _securityService;

        public ProfilesController(ISecurityService securityService, ISettingsManager settingsManager)
        {
            _securityService = securityService;
            _settingsManager = settingsManager;
        }

        [HttpGet]
        [Route("currentuser")]
        [ResponseType(typeof(webModel.Setting[]))]
        public IHttpActionResult GetCurrentUserProfile()
        {
            return Ok(GetCurrentUserProfileInternal().Settings.Select(setting => setting.ToWebModel()).ToArray());
        }

        [HttpPost]
        [Route("currentuser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateCurrentUserProfile(webModel.Setting[] settings)
        {
            var userProfile = GetCurrentUserProfileInternal();
            userProfile.Settings = settings.Select(setting => setting.ToModuleModel()).ToArray();
            lock (_lock)
            {
                _settingsManager.SaveEntitySettingsValues(userProfile);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private UserProfile GetCurrentUserProfileInternal()
        {
            var currentUser = _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full).Result;
            var userProfile = new UserProfile(currentUser.Id);
            lock (_lock)
            {
                userProfile.Settings = _settingsManager.GetModuleSettings("VirtoCommerce.Platform").Where(setting => setting.GroupName == "User profile").ToArray();
                _settingsManager.LoadEntitySettingsValues(userProfile);
            }
            return userProfile;
        }
    }
}