#region
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("pages")]
    public class PageController : BaseController
    {
        #region Public Methods and Operators
        [Route("{page}")]
        // GET: Pages
        public ActionResult DisplayPage(string page)
        {
            var model = new PagesService().GetPage(page);
            this.Context.Set("page", model);

            return View(model.Layout);
        }
        #endregion
    }
}