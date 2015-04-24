using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
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
		[ResponseType(typeof(webModel.ModuleDescriptor[]))]
		[Route("modules")]
		public IHttpActionResult GetModules()
		{
			var retVal = _settingManager.GetModules();
			return Ok(retVal.Select(x=>x.ToWebModel()).ToArray());
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
			var retVal = _settingManager.GetSettings(id);
			return Ok(retVal.Select(x=>x.ToWebModel()).ToArray());
		}

		[HttpPost]
		[Route("")]
		public void Update(webModel.Setting[] settings)
		{
			_settingManager.SaveSettings(settings.Select(x=>x.ToModuleModel()).ToArray());
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