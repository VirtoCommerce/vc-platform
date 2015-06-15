#region
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using LibSassNet;
using VirtoCommerce.Web.Models.Helpers;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("")]
    public class AssetsController : StoreControllerBase
    {
        #region Fields
        private readonly ISassCompiler _compiler = new SassCompiler();
        #endregion

        #region Public Properties
        public Hash Settings
        {
            get
            {
                return Hash.FromAnonymousObject(new { settings = this.Context.Settings });
            }
        }
        #endregion

        #region Public Methods and Operators
        public static string RenderPartialViewToString(Controller thisController, string viewName, object model)
        {
            // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
            thisController.ViewData.Model = model;

            // initialize a string builder
            using (var sw = new StringWriter())
            {
                // find and load the view or partial view, pass it through the controller factory
                var viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                var viewContext = new ViewContext(
                    thisController.ControllerContext,
                    viewResult.View,
                    thisController.ViewData,
                    thisController.TempData,
                    sw);

                // render it
                viewResult.View.Render(viewContext, sw);

                //return the razorized view/partial-view as a string
                return sw.ToString();
            }
        }

        [OutputCache(CacheProfile = "AssetsCachingProfile")]
        [Route("global/assets/{*assetId}")]
        public ActionResult Global(string assetId)
        {
            var virtualPath = String.Format("~/App_Data/Themes/_Global/{0}", assetId);
            return this.AssetResult(virtualPath, assetId);
        }

        [OutputCache(CacheProfile = "AssetsCachingProfile")]
        [Route("images/{assetId}")]
        public ActionResult Images(string assetId)
        {
            var virtualPath = String.Format("~/App_Data/Images/{0}", assetId);
            return this.AssetResult(virtualPath, assetId);
        }

        [OutputCache(CacheProfile = "AssetsCachingProfile")]
        [Route("themes/assets/{asset}")]
        public ActionResult Themed(string theme, string asset)
        {
            var virtualPath = String.Format("~/App_Data/Themes/{0}/assets/{1}", this.Context.Theme.Path, asset);
            return this.AssetResult(virtualPath, asset);
        }
        #endregion

        #region Methods
        private ActionResult AssetResult(string virtualPath, string assetId)
        {
            //Response.Cache.SetMaxAge(TimeSpan.FromDays(365));

            if (HostingEnvironment.VirtualPathProvider.FileExists(virtualPath))
            {
                return new DownloadResult(virtualPath);
            }

            if (assetId.EndsWith("scss.css"))
            {
                var compiledContent = this.GetScss(virtualPath, assetId);
                return this.Content(compiledContent, "text/css");
            }

            return new ExtensionPartialView(assetId, this.Settings);
        }

        private string GetScss(string virtualPath, string id)
        {
            var contextKey = String.Format("vc-cms-{0}", virtualPath);
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                return value as string;
            }

            var str = RenderPartialViewToString(this, id.Replace("scss.css", "scss"), this.Settings);

            var compiledContent = this._compiler.Compile(str, OutputStyle.Compressed, false);

            var rootVirtual = String.Format("~/App_Data/Themes/{0}", this.Context.Theme.Path);
            var rootPath = this.Server.MapPath(rootVirtual);
            
            var allDirectories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
            HttpRuntime.Cache.Insert(contextKey, compiledContent, new CacheDependency(allDirectories));

            // create a copy of asset in the folder
            return compiledContent;
        }
        #endregion
    }
}