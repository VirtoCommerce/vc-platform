using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using VirtoCommerce.Web.Client.Extensions.Filters.Caching;

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
		[CustomDonutOutputCache(CacheProfile = "HomeCache")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
