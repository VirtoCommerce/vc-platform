using System.Web.Mvc;

namespace VirtoCommerce.Web.Controllers
{
    public class ErrorsController : StoreControllerBase
    {
        [HttpGet]
        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 404;

            return View("404");
        }
    }
}