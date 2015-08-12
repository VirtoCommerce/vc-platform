using System.Linq;
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
        [ResponseType(typeof(webModel.Setting[]))]
        [Route("")]
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
        [ResponseType(typeof(webModel.Setting[]))]
        [Route("modules/{id}")]
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
        [HttpGet]
        [ResponseType(typeof(webModel.Setting))]
        [Route("{id}")]
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
        [CheckPermission(Permission = PredefinedPermissions.SettingManage)]
        public void Update(webModel.Setting[] settings)
        {
            _settingsManager.SaveSettings(settings.Select(x => x.ToModuleModel()).ToArray());
        }

        /// <summary>
        /// Get non-array setting value by name
        /// </summary>
        /// <param name="name">Setting system name.</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(object))]
        [Route("value/{name}")]
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
        [ResponseType(typeof(object[]))]
        [Route("values/{name}")]
        public IHttpActionResult GetArray(string name)
        {
            var value = _settingsManager.GetArray<object>(name, null);
            return Ok(value);
        }
    }
}
