using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Web.Model;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/common")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : Controller
    {
        private readonly IPlatformMemoryCache _memoryCache;

        public CommonController(IPlatformMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// Get Full list of countries defined by ISO 3166-1 alpha-3
        /// based on https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
        /// </summary>
        [HttpGet]
        [Route("countries")]
        [AllowAnonymous]
        public ActionResult<List<Country>> GetCountries()
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetCountries));
            var results = _memoryCache.GetOrCreateExclusive(cacheKey, (cacheEntry) =>
            {
                var filePath = Path.GetFullPath("app_data/countries.json");
                return JsonConvert.DeserializeObject<List<Country>>(System.IO.File.ReadAllText(filePath));
            });

            return Ok(results);
        }
    }
}
