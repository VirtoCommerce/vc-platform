using System.Web;
using System.Web.Mvc;
using VirtoCommerce.LiquidThemeEngine;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "AssetsCachingProfile")]
    public class AssetController : Controller
    {
        private readonly ILiquidThemeEngine _themeEngine;

        public AssetController(ILiquidThemeEngine themeEngine)
        {
            _themeEngine = themeEngine;
        }

        /// <summary>
        /// GET: /themes/assets/{*asset}
        /// Handles all asset requests because it may be liquid and scss files which should be preprocessed
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAssets(string asset)
        {
            var stream = _themeEngine.GetAssetStream(asset);
            if (stream != null)
            {
                return File(stream, MimeMapping.GetMimeMapping(asset));
            }
            throw new HttpException(404, asset);
        }

        /// <summary>
        /// GET: /themes/global/assets/{*asset}
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGlobalAssets(string asset)
        {
            var stream = _themeEngine.GetAssetStream(asset, searchInGlobalThemeOnly: true);
            if (stream != null)
            {
                return File(stream, MimeMapping.GetMimeMapping(asset));
            }
            throw new HttpException(404, asset);
        }
    }
}
