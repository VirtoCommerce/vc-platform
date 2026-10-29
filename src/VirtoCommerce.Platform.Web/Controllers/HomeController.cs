using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Web.Model.Home;

namespace VirtoCommerce.Platform.Web.Controllers
{
    /// <summary>
    /// The platform ships no admin UI: the back office is delivered by a UI module
    /// (VirtoCommerce.AdminUI by default), which maps its own page at the site root.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
