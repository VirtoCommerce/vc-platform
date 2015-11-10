using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Controllers
{
    public class AccountController : Controller
    {
        private readonly WorkContext _workContext;
        public AccountController(WorkContext workContext)
        {
            _workContext = workContext;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        [Route("account/login")]
        public ActionResult Login()
        {
            _workContext.Login = new Login();
             return View("customers/login", _workContext);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("account/login")]
        public ActionResult Login(Login loginModel, string returnUrl)
        {
            ModelState.AddModelError("", "email");

            return View("customers/login", _workContext);
        }
    }
}