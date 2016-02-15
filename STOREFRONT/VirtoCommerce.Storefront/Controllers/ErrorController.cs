using System.Net;
using System.Web.Mvc;

namespace VirtoCommerce.Storefront.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Do not rename this method because it have references in Global.asax and SeoRoute.cs
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post | HttpVerbs.Put)]
        public ActionResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return View("404");
        }
    }
}