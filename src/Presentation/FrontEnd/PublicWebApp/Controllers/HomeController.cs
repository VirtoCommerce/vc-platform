using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiWebClient.Caching;

namespace VirtoCommerce.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        [DonutOutputCache(CacheProfile = "HomeCache")]
        public ActionResult Index()
        {
            return View();
        }
    }
}