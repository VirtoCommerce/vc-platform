using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Model.Settings;
using webModel = VirtoCommerce.Platform.Web.Model.Settings;
using moduleModel = VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Converters.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/settings")]
    public class SettingController : ApiController
    {
        private readonly ISettingsManager _settingsManager;
        public SettingController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// api/settings/modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [Route("modules")]
        public IHttpActionResult GetModules()
        {
            var retVal = _settingsManager.GetModules();
            return Ok(retVal.Select(x => x.ToWebModel()).ToArray());
        }

        /// <summary>
        /// api/settings/modules/123
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.Setting[]))]
        [Route("modules/{id}")]
        public IHttpActionResult GetModuleSettings(string id)
        {
            var retVal = _settingsManager.GetSettings(id);
            return Ok(retVal.Select(x => x.ToWebModel()).ToArray());
        }

        [HttpPost]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.SettingManage)]
        public void Update(webModel.Setting[] settings)
        {
            _settingsManager.SaveSettings(settings.Select(x => x.ToModuleModel()).ToArray());
        }

        /// <summary>
        /// api/settings/value/name
        /// </summary>
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
        /// api/settings/values/name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(object))]
        [Route("values/{name}")]
        public IHttpActionResult GetArray(string name)
        {
            var value = _settingsManager.GetArray<object>(name, null);
            return Ok(value);
        }
    }
}