using System.Web.Mvc;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Virto.Helpers.MVC;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class HomeController.
	/// </summary>
	[Localize]
	public class HomeController : ControllerBase
    {
		/// <summary>
		/// Home page
		/// </summary>
		/// <returns>ActionResult.</returns>
		[CustomOutputCache(CacheProfile = "HomeCache")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
