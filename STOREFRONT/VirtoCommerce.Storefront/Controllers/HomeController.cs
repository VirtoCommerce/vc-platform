using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Controllers
{
    public class HomeController : Controller
    {
        private readonly WorkContext _workContext;
        public HomeController(WorkContext context)
        {
            _workContext = context;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View("index", _workContext);
        }
    }
}