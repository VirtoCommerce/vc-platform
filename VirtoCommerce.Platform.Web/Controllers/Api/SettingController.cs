using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Converters.Settings;
using webModel = VirtoCommerce.Platform.Web.Model.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/settings")]
    public class SettingController : ApiController
    {
        private readonly ISettingsManager _settingsManager;
        public SettingController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(webModel.Setting[]))]
        public IHttpActionResult GetAllSettings()
        {
            var modules = _settingsManager.GetModules();
            return Ok(modules.SelectMany(x => _settingsManager.GetModuleSettings(x.Id)).Select(x => x.ToWebModel()).ToArray());
        }

        /// <summary>
        /// Get settings registered by specific module
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("modules/{id}")]
        [ResponseType(typeof(webModel.Setting[]))]
        public IHttpActionResult GetModuleSettings(string id)
        {
            var retVal = _settingsManager.GetModuleSettings(id);
            return Ok(retVal.Select(x => x.ToWebModel()).ToArray());
        }

        /// <summary>
        /// Get setting details by name
        /// </summary>
        /// <param name="id">Setting system name.</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Setting not found.</response>
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(webModel.Setting))]
        public IHttpActionResult GetSetting(string id)
        {
            var retVal = _settingsManager.GetSettingByName(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel());
            }
            return NotFound();
        }

        /// <summary>
        /// Update settings values
        /// </summary>
        /// <param name="settings"></param>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.SettingUpdate)]
        public IHttpActionResult Update(webModel.Setting[] settings)
        {
            _settingsManager.SaveSettings(settings.Select(x => x.ToModuleModel()).ToArray());
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get non-array setting value by name
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("value/{name}")]
        [ResponseType(typeof(object))] // Produces invalid response type in generated client
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetValue(string name)
        {
            var value = _settingsManager.GetValue<object>(name, null);
            return Ok(value);
        }

        /// <summary>
        /// Get array setting values by name
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("values/{name}")]
        [ResponseType(typeof(object[]))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetArray(string name)
        {
            var value = _settingsManager.GetArray<object>(name, null);
            return Ok(value);
        }
    }
}
