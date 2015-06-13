using System;
using System.Net;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Controllers
{
    [HandleError]
    public class ErrorsController : StoreControllerBase
    {
        public ActionResult Index(int code)
        {
            Response.StatusCode = code;
            return View(code);
        }

        [HttpGet]
        public ActionResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return View("404");
        }

        [HttpGet]
        public ActionResult Http500(Exception exception)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Context.ErrorMessage = string.Format(
                "<p>{0}</p><p>{1}</p>", exception.Message, exception.StackTrace);

            return View("error");
        }
    }
}