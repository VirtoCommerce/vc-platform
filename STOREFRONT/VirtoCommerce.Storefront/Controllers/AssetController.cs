using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using LibSassNetProxy;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.Storefront.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "AssetsCachingProfile")]
    public class AssetController : Controller
    {
        private readonly ShopifyLiquidThemeEngine _themeAdaptor;
        public AssetController(ShopifyLiquidThemeEngine themeAdaptor)
        {
            _themeAdaptor = themeAdaptor;
        }

        #region Public Methods and Operators

        /// <summary>
        /// GET: /themes/assets/{asset}
        /// Need handle all assets requests because it may be liquid and scss files which should be preprocessed
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAssets(string asset)
        {
            var stream = _themeAdaptor.GetAssetStream(asset);
            if(stream != null)
            {
                return base.File(stream, MimeMapping.GetMimeMapping(asset));
            }
            return HttpNotFound(asset);
        }

        #endregion

    }
}
