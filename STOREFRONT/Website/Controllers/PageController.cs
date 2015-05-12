#region

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
            var model = new PagesService().GetPage(context, page);

            if(model == null)
                throw new HttpException(404, "NotFound");

            this.Context.Set("page", model);

            return View(model.Layout ?? "page");
        }
        #endregion
    }
}