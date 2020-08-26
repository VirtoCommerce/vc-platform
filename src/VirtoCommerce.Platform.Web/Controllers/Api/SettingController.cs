using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/settings")]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ISettingsSearchService _settingsSearchService;

        public SettingController(ISettingsManager settingsManager, ISettingsSearchService settingsSearchService)
        {
            _settingsManager = settingsManager;
            _settingsSearchService = settingsSearchService;
        }

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult<ObjectSettingEntry>> GetAllGlobalSettings()
        {
            var result = await _settingsManager.GetObjectSettingsAsync(_settingsManager.AllRegisteredSettings.Where(x => !x.IsHidden).Select(x => x.Name));
            return Ok(result);
        }

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("modules/{id}")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult<ObjectSettingEntry[]>> GetGlobalModuleSettingsAsync(string id)
        {
            var criteria = new SettingsSearchCriteria
            {
                ModuleId = id,
                Take = int.MaxValue
            };
            var result = await _settingsSearchService.SearchSettingsAsync(criteria);
            return Ok(result.Results.Where(x=>!x.IsHidden).ToList());
        }

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        [Authorize(PlatformConstants.Security.Permissions.SettingAccess)]
        public async Task<ActionResult<ObjectSettingEntry>> GetGlobalSettingAsync(string name)
        {
            var result = await _settingsManager.GetObjectSettingAsync(name);
            return Ok(result);
        }

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <param name="objectSettings"></param>
        [HttpPost]
        [Route("")]
        [Authorize(PlatformConstants.Security.Permissions.SettingUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateAsync([FromBody] ObjectSettingEntry[] objectSettings)
        {
            using (await AsyncLock.GetLockByKey("settings").LockAsync())
            {
                await _settingsManager.SaveObjectSettingsAsync(objectSettings);
            }
            return NoContent();
        }

        /// <summary>
        /// Get array setting values by name
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("values/{name}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<object[]>> GetArrayAsync(string name)
        {
            var setting = await _settingsManager.GetObjectSettingAsync(name);
            object[] result = null;
            if (setting != null)
            {
                if (!setting.AllowedValues.IsNullOrEmpty())
                {
                    result = setting.AllowedValues;
                }
                else if (setting.Value != null)
                {
                    result = new[] { setting.Value };
                }
                else if (setting.DefaultValue != null)
                {
                    result = new[] { setting.DefaultValue };
                }

            }
            return Ok(result);
        }

        /// <summary>
        /// Get UI customization setting
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("ui/customization")]
        public async Task<ActionResult<ObjectSettingEntry>> GetUICustomizationSetting()
        {
            var result = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.UserInterface.Customization.Name);
            return Ok(result);
        }
    }
}
