using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/common")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : Controller
    {
        private readonly ICountriesService _countriesService;

        public CommonController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [HttpGet]
        [Route("countries")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Country>>> GetCountries()
        {
            var results = await _countriesService.GetCountriesAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("ping")]
        [AllowAnonymous]
        public ActionResult<string> Ping()
        {
            return Ok("pong");
        }
    }
}
