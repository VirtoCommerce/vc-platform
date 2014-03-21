using System.Globalization;
using System.Web.Mvc;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class SettingsController.
	/// </summary>
	public class SettingsController : ControllerBase
    {

		/// <summary>
		/// Localizes the specified text. Called via ajax to localize
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>JsonResult.</returns>
		[HttpGet]
		public JsonResult Localize(string text, string category = "", string culture = null)
		{
			CultureInfo cultureInfo = null;

	        if (culture != null)
	        {
		        try
		        {
			        cultureInfo = CultureInfo.CreateSpecificCulture(culture);
		        }
		        catch
		        {
			        cultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
		        }
	        }

			return Json(new { translation = text.Localize(null, category, cultureInfo) }, JsonRequestBehavior.AllowGet);
		}
    }
}
