using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/settings/v2")]
    [Authorize]
    public class SettingsV2Controller : Controller
    {
        private readonly ISettingsPropertyService _settingsPropertyService;
        private readonly UserManager<ApplicationUser> _userManager;

        // Serializer settings that preserve dictionary keys as-is (no camelCase).
        // The global PolymorphJsonContractResolver (CamelCasePropertyNamesContractResolver)
        // lowercases dictionary keys, which breaks setting Name matching.
        private static readonly JsonSerializerSettings _preserveKeysSettings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy()
            },
            Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public SettingsV2Controller(
            ISettingsPropertyService settingsPropertyService,
            UserManager<ApplicationUser> userManager)
        {
            _settingsPropertyService = settingsPropertyService;
            _userManager = userManager;
        }

        #region Global endpoints

        /// <summary>
        /// Get global settings schema (metadata only, no values)
        /// </summary>
        [HttpGet("global/schema")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult<IReadOnlyList<SettingPropertySchema>>> GetGlobalSchema(
            [FromQuery] string moduleId = null,
            [FromQuery] string keyword = null)
        {
            var criteria = AbstractTypeFactory<SettingsPropertySearchCriteria>.TryCreateInstance();
            criteria.ModuleId = moduleId;
            criteria.Keyword = keyword;

            var result = await _settingsPropertyService.GetSchemaAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get global settings values as a flat { name: value } dictionary.
        /// Optional <paramref name="moduleId"/> narrows the response to a single
        /// module's settings — same filter as <see cref="GetGlobalSchema"/>, so
        /// a frontend can read+use one module's values in one round-trip
        /// without a follow-up /schema lookup.
        /// </summary>
        [HttpGet("global/values")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult> GetGlobalValues(
            [FromQuery] bool modifiedOnly = false,
            [FromQuery] string moduleId = null)
        {
            var result = await _settingsPropertyService.GetValuesAsync(
                modifiedOnly: modifiedOnly,
                moduleId: moduleId);
            return new JsonResult(result, _preserveKeysSettings);
        }

        /// <summary>
        /// Update global settings values.
        /// When replaceAll is true, the dictionary is the complete set of desired modifications —
        /// any currently-modified setting not in the dictionary is reset to its default value.
        /// An empty dictionary with replaceAll=true resets all settings to defaults.
        /// </summary>
        [HttpPost("global/values")]
        [Authorize(PlatformConstants.Security.Permissions.SettingUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveGlobalValues(
            [FromBody] Dictionary<string, object> values,
            [FromQuery] bool replaceAll = false)
        {
            if ((values == null || values.Count == 0) && !replaceAll)
            {
                return BadRequest("Request body must be a non-empty JSON object");
            }

            values ??= new Dictionary<string, object>();

            using (await AsyncLock.GetLockByKey("settings").LockAsync())
            {
                await _settingsPropertyService.SaveValuesAsync(values, replaceAll: replaceAll);
            }

            return NoContent();
        }

        #endregion

        #region Tenant endpoints

        /// <summary>
        /// Get tenant settings schema (metadata only, no values).
        /// Schema depends only on tenantType registration, not on a specific tenant instance.
        /// </summary>
        [HttpGet("tenant/{tenantType}/schema")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult<IReadOnlyList<SettingPropertySchema>>> GetTenantSchema(
            string tenantType,
            [FromQuery] string moduleId = null,
            [FromQuery] string keyword = null)
        {
            var criteria = AbstractTypeFactory<SettingsPropertySearchCriteria>.TryCreateInstance();
            criteria.ModuleId = moduleId;
            criteria.Keyword = keyword;

            var result = await _settingsPropertyService.GetSchemaAsync(criteria, tenantType);
            return Ok(result);
        }

        /// <summary>
        /// Get tenant settings values as a flat { name: value } dictionary.
        /// Optional <paramref name="moduleId"/> narrows the response to a single
        /// module's settings.
        /// </summary>
        [HttpGet("tenant/{tenantType}/{tenantId}/values")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult> GetTenantValues(
            string tenantType,
            string tenantId,
            [FromQuery] bool modifiedOnly = false,
            [FromQuery] string moduleId = null)
        {
            var result = await _settingsPropertyService.GetValuesAsync(
                tenantType,
                tenantId,
                modifiedOnly,
                moduleId);
            return new JsonResult(result, _preserveKeysSettings);
        }

        /// <summary>
        /// Update tenant settings values.
        /// When replaceAll is true, the dictionary is the complete set of desired modifications —
        /// any currently-modified setting not in the dictionary is reset to its default value.
        /// </summary>
        [HttpPost("tenant/{tenantType}/{tenantId}/values")]
        [Authorize(PlatformConstants.Security.Permissions.SettingUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveTenantValues(
            string tenantType,
            string tenantId,
            [FromBody] Dictionary<string, object> values,
            [FromQuery] bool replaceAll = false)
        {
            if ((values == null || values.Count == 0) && !replaceAll)
            {
                return BadRequest("Request body must be a non-empty JSON object");
            }

            values ??= new Dictionary<string, object>();

            using (await AsyncLock.GetLockByKey("settings").LockAsync())
            {
                await _settingsPropertyService.SaveValuesAsync(values, tenantType, tenantId, replaceAll);
            }

            return NoContent();
        }

        #endregion

        #region Me endpoints (current user's UserProfile-scoped settings)

        // The /me/* endpoints are a self-service shortcut over the
        // /tenant/UserProfile/{userId}/* tenant endpoints: they resolve
        // {userId} from the authenticated caller's claims so a frontend
        // never needs to know its own user id, and they require only
        // authentication — your own profile settings are yours by default,
        // no extra Setting* permission needed. Anonymous callers get
        // 401 from the class-level [Authorize].

        /// <summary>
        /// Get the current user's UserProfile settings schema (metadata only).
        /// Schema is the same for every user — it's defined by the
        /// platform's UserProfile-tenant registration. Optionally filter
        /// by moduleId so a frontend only fetches its own module's
        /// per-user settings.
        /// </summary>
        [HttpGet("me/schema")]
        public async Task<ActionResult<IReadOnlyList<SettingPropertySchema>>> GetMeSchema(
            [FromQuery] string moduleId = null,
            [FromQuery] string keyword = null)
        {
            var criteria = AbstractTypeFactory<SettingsPropertySearchCriteria>.TryCreateInstance();
            criteria.ModuleId = moduleId;
            criteria.Keyword = keyword;

            var result = await _settingsPropertyService.GetSchemaAsync(criteria, nameof(UserProfile));
            return Ok(result);
        }

        /// <summary>
        /// Get the current user's effective UserProfile settings values
        /// as a flat { name: value } dictionary. Optional <paramref name="moduleId"/>
        /// narrows the response to a single module's per-user settings — same
        /// filter <see cref="GetMeSchema"/> accepts, so a frontend can do a
        /// single round-trip read without a follow-up /me/schema lookup.
        /// </summary>
        [HttpGet("me/values")]
        public async Task<ActionResult> GetMeValues(
            [FromQuery] bool modifiedOnly = false,
            [FromQuery] string moduleId = null)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                // [Authorize] should have already ensured this, but a missing
                // sub claim from a malformed token would land here.
                return Unauthorized();
            }

            var result = await _settingsPropertyService.GetValuesAsync(
                nameof(UserProfile),
                userId,
                modifiedOnly,
                moduleId);
            return new JsonResult(result, _preserveKeysSettings);
        }

        /// <summary>
        /// Update the current user's UserProfile settings values.
        /// When replaceAll is true, the dictionary is the complete set of
        /// desired modifications — any currently-modified setting not in
        /// the dictionary is reset to its default value.
        /// </summary>
        [HttpPost("me/values")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveMeValues(
            [FromBody] Dictionary<string, object> values,
            [FromQuery] bool replaceAll = false)
        {
            if ((values == null || values.Count == 0) && !replaceAll)
            {
                return BadRequest("Request body must be a non-empty JSON object");
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            values ??= new Dictionary<string, object>();

            using (await AsyncLock.GetLockByKey("settings").LockAsync())
            {
                await _settingsPropertyService.SaveValuesAsync(values, nameof(UserProfile), userId, replaceAll);
            }

            return NoContent();
        }

        #endregion
    }
}
