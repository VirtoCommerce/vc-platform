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
            return View();
        }
    }
}
