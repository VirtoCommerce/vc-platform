using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/common")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : Controller
    {
        private readonly ICountriesService _countriesService;
        private readonly LoginPageUIOptions _loginPageOptions;

        public CommonController(ICountriesService countriesService,
            IOptions<LoginPageUIOptions> loginPageOptions)
        {
            _countriesService = countriesService;
            _loginPageOptions = loginPageOptions.Value;
        }


        [HttpGet]
        [Route("countries")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Country>>> GetCountries()
        {
            var results = await _countriesService.GetCountriesAsync();
            return Ok(results);
        }

        [HttpGet("countries/{countryCode}/regions")]
        public async Task<ActionResult<CountryRegion[]>> GetCountryRegionsAsync(string countryCode)
        {
            var results = await _countriesService.GetCountryRegionsAsync(countryCode);
            return Ok(results);
        }

        /// <summary>
        /// Get predefined login page background opitons
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("ui/loginPageOptions")]
        public ActionResult GetLoginPageUIOptions()
        {
            // process login page predefined options
            string backgrounUrl = _loginPageOptions.BackgroundUrl;
            string patternUrl = _loginPageOptions.PatternUrl;

            if (!string.IsNullOrWhiteSpace(_loginPageOptions.PresetName))
            {
                var preset = _loginPageOptions.Presets?.FirstOrDefault(x => x.Name.EqualsInvariant(_loginPageOptions.PresetName));
                backgrounUrl = preset?.BackgroundUrl;
                patternUrl = preset?.PatternUrl;
            }

            return Ok(new { BackgroundUrl = backgrounUrl, PatternUrl = patternUrl });
        }
    }
}
