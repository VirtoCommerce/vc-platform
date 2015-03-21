using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.Framework.Web.Settings;


namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [RoutePrefix("api/settings")]
    public class SettingController : ApiController
    {
        private readonly ISettingsManager _settingManager;
        public SettingController(ISettingsManager settingManager)
        {
            _settingManager = settingManager;
        }

        /// <summary>
        /// api/settings/modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ModuleDescriptor[]))]
        [Route("modules")]
        public IHttpActionResult GetModules()
        {
            var retVal = _settingManager.GetModules();
            return Ok(retVal);
        }

        /// <summary>
        /// api/settings/modules/123
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(SettingDescriptor[]))]
        [Route("modules/{id}")]
        public IHttpActionResult GetModuleSettings(string id)
        {
            var retVal = _settingManager.GetSettings(id);
            return Ok(retVal);
        }

        [HttpPost]
        [Route("")]
        public void Update(SettingDescriptor[] settings)
        {
            _settingManager.SaveSettings(settings);
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
            var value = _settingManager.GetValue<object>(name, null);
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
            var value = _settingManager.GetArray<object>(name, null);
            return Ok(value);
        }
    }
}
