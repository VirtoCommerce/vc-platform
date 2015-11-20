using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("Errors")]
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("404")]
        public ActionResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return View("404");
        }

    }
}