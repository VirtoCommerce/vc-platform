using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Model.Profiles;

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
        [ResponseType(typeof(UserProfile))]
        public async Task<IHttpActionResult> GetCurrentUserProfile()
        {
            var currentUser = await _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full);
            var userProfile = new UserProfile(currentUser.Id);
            lock (_lock)
            {
                var modules = _settingsManager.GetModules();
                userProfile.Settings = modules.SelectMany(module => _settingsManager.GetModuleSettings(module.Id)).Where(setting => setting.GroupName == "Platform|User Profile").ToArray();
                _settingsManager.LoadEntitySettingsValues(userProfile);
            }
            return Ok(userProfile);
        }

        [HttpPost]
        [Route("currentuser")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateCurrentUserProfile(UserProfile userProfile)
        {
            var currentUser = await _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full);
            if (currentUser.Id != userProfile.Id)
            {
                return Unauthorized();
            }
            lock (_lock)
            {
                _settingsManager.SaveEntitySettingsValues(userProfile);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}