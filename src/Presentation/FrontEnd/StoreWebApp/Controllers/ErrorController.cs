using System.Web.Mvc;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class ErrorController.
	/// </summary>
	public class ErrorController : Controller
    {
        //
        // GET: /Error/

		/// <summary>
		/// Default  error page
		/// </summary>
		/// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Oops");
        }

		/// <summary>
		/// Store is closed.
		/// </summary>
		/// <returns>ActionResult.</returns>
        public ActionResult StoreClosed()
        {
            //Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }


		/// <summary>
		/// Return the Oops error page.
		/// </summary>
		/// <returns>ActionResult.</returns>
        public ActionResult Oops()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }


		/// <summary>
		/// Return the error page Fail Whale...
		/// </summary>
		/// <returns>ActionResult.</returns>
        public ActionResult FailWhale()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

    }
}
