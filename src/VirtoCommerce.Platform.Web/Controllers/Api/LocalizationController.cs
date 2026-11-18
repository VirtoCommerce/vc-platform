using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Localizations;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/localization")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LocalizationController : Controller
    {
        private readonly ITranslationService _translationService;
        private readonly IRegionalSettingsService _regionalSettingsService;

        public LocalizationController(ITranslationService translationService, IRegionalSettingsService regionalSettingsService)
        {
            _translationService = translationService;
            _regionalSettingsService = regionalSettingsService;
        }

        /// <summary>
        /// Return localization resource
        /// </summary>
        /// <param name="lang">Language of localization resource (en by default)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public ActionResult<JObject> GetLocalization(string lang = null)
        {
            var result = _translationService.GetTranslationDataForLanguage(lang);

            return Ok(result);
        }

        /// <summary>
        /// Return all available locales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("locales")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public ActionResult<string[]> GetLocales()
        {
            var locales = _translationService.GetListOfInstalledLanguages();

            return Ok(locales);
        }

        /// <summary>
        /// Return all available regional formats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("regionalformats")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<string>>> GetRegionalFormats()
        {
            var formats = await _regionalSettingsService.GetRegionalFormatsAsync();

            return Ok(formats);
        }

        /// <summary>
        /// Return all available time zones (IANA ids)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("timezones")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<string>>> GetTimeZones()
        {
            var timeZones = await _regionalSettingsService.GetTimeZonesAsync();

            return Ok(timeZones);
        }
    }
}
