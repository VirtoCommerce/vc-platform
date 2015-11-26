using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.Storefront.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Do not rename this method because it have references in Global.asax and SeoRoute.cs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return View("404");
        }

    }
}