using System.Web.Mvc;
using VirtoCommerce.Web.Client.Caching;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class HomeController.
    /// </summary>
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "HomeCache")]
        public ActionResult Index()
        {
            if (Request.Path.EndsWith("realhome"))
            {
                return View();
            }
            else
            {
                return new RedirectResult("../index.html", true);
            }
        }
    }
}
