using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using LibSassNetProxy;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Exceptions;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("")]
    public class AssetsController : Controller
    {
        private readonly SassCompilerProxy _compiler = new SassCompilerProxy();
        private readonly ShopifyLiquidThemeStructure _themeAdaptor;
        public AssetsController(ShopifyLiquidThemeStructure themeAdaptor)
        {
            _themeAdaptor = themeAdaptor;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Need handle all assets requests because it may be liquid and scss files which should be preprocessed
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "AssetsCachingProfile")]
        [Route("themes/assets/{asset}")]
        public ActionResult Themed(string theme, string asset)
        {
            var virtualPath = String.Format("~/App_Data/Themes/{0}/assets/{1}", _themeAdaptor.ThemeName, asset);
            return AssetResult(virtualPath, asset);
        }

        #endregion

        #region Methods

        private ActionResult AssetResult(string virtualPath, string assetId)
        {
            if (HostingEnvironment.VirtualPathProvider.FileExists(virtualPath))
            {
                return new DownloadResult(virtualPath);
            }
            else
            {
                var fileExtensions = System.IO.Path.GetExtension(assetId);
                var contentType = "application/octet-stream";
                if (!string.IsNullOrEmpty(fileExtensions) && FileExtensionMapper.Contains(fileExtensions))
                {
                    contentType = FileExtensionMapper.GetContentType(fileExtensions);
                }

                assetId = assetId.Replace(".scss.css", ".scss");
                var settings = _themeAdaptor.GetSettings("''");
                //Try to parse liquid asset resource
                var content = _themeAdaptor.RenderTemplateByName(assetId, new Dictionary<string, object>() { { "settings", settings } });

                if (assetId.EndsWith(".scss"))
                {
                    try
                    {
                        //handle scss resources
                        content = _compiler.Compile(content);
                    }
                    catch (Exception ex)
                    {
                        throw new SaasCompileException(assetId, content, ex);
                    }
                }

                return Content(content, contentType);
            }

        }

        #endregion
    }
}
