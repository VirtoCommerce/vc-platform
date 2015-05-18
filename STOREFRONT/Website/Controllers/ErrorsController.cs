using System.Net;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Controllers
{
    public class ErrorsController : StoreControllerBase
    {
        [HttpGet]
        public ActionResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return View("404");
        }
    }
}