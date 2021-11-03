using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Model.Profiles;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/profiles")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ILocalModuleCatalog _moduleCatalog;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ProfilesController(UserManager<ApplicationUser> userManager, ISettingsManager settingsManager, ILocalModuleCatalog moduleCatalog, IConfiguration configuration)
        {
            _userManager = userManager;
            _settingsManager = settingsManager;
            _moduleCatalog = moduleCatalog;
            _configuration = configuration;
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("currentuser")]
        public async Task<ActionResult> GetCurrentUserProfileAsync()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser != null)
            {
                var userProfile = AbstractTypeFactory<UserProfile>.TryCreateInstance();
                userProfile.Id = currentUser.Id;
                await _settingsManager.DeepLoadSettingsAsync(userProfile);

                // Main menu settings at initial boot
                var nameMainMenuState = PlatformConstants.Settings.UserProfile.MainMenuState.Name;
                if (userProfile.Settings.FirstOrDefault(x => x.Name == nameMainMenuState)?.Value == null)
                {
                    var settingMenuState = new DefaultMainMenuState();
                    _configuration.GetSection("DefaultMainMenuState").Bind(settingMenuState);
                    var serializeOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var mainMenuState = JsonSerializer.Serialize(settingMenuState, serializeOptions);
                    userProfile.Settings.FirstOrDefault(x => x.Name == nameMainMenuState).Value = mainMenuState;
                }

                return Ok(userProfile);
            }
            return Ok();
        }

        /// <summary>
        /// Update current user profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("currentuser")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateCurrentUserProfileAsync([FromBody] UserProfile userProfile)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.Id != userProfile.Id)
            {
                return Unauthorized();
            }
            using (await AsyncLock.GetLockByKey(userProfile.ToString()).GetReleaserAsync())
            {
                await _settingsManager.DeepSaveSettingsAsync(userProfile);
            }
            return NoContent();
        }
    }
}
