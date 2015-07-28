#region

using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Services;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    //[RoutePrefix("pages")]
    public class PageController : StoreControllerBase
    {
        #region Public Methods and Operators
        //[Route("{page}")]
        // GET: Pages
        public async Task<ActionResult> DisplayPageAsync(string page)
        {
            var context = SiteContext.Current;
            var model = await Task.FromResult(new PagesService().GetPage(context, page));

            if (model == null)
                throw new HttpException(404, "NotFound");

            Context.Set("page", model);

            return View(model.Layout ?? "page");
        }

        [Route("~/files/{*asset}")]
        public async Task<ActionResult> DisplayPageAsset(string asset)
        {
            var virtualPath = await Task.FromResult(string.Format("~/App_Data/Pages/{0}/{1}", Context.StoreId, asset));
            return new DownloadResult(virtualPath);
        }
        #endregion
    }
}