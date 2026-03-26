using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/settings/v2")]
    [Authorize]
    public class SettingsV2Controller : Controller
    {
        private readonly ISettingsPropertyService _settingsPropertyService;

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

        public SettingsV2Controller(ISettingsPropertyService settingsPropertyService)
        {
            _settingsPropertyService = settingsPropertyService;
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
        /// Get global settings values as a flat { name: value } dictionary
        /// </summary>
        [HttpGet("global/values")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult> GetGlobalValues(
            [FromQuery] bool modifiedOnly = false)
        {
            var result = await _settingsPropertyService.GetValuesAsync(modifiedOnly: modifiedOnly);
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
        /// Get tenant settings values as a flat { name: value } dictionary
        /// </summary>
        [HttpGet("tenant/{tenantType}/{tenantId}/values")]
        [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
        public async Task<ActionResult> GetTenantValues(
            string tenantType,
            string tenantId,
            [FromQuery] bool modifiedOnly = false)
        {
            var result = await _settingsPropertyService.GetValuesAsync(tenantType, tenantId, modifiedOnly);
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
    }
}
