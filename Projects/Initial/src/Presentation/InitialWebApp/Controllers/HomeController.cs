using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Client.Caching;

namespace Initial.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        //
        // GET: /Home/

        [DonutOutputCache(CacheProfile = "HomeCache")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
