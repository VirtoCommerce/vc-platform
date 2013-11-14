using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using VirtoCommerce.Client;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers.MVC;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class BannerController.
	/// </summary>
	[Localize]
	public class BannerController : ControllerBase
    {
		/// <summary>
		/// The _content helper
		/// </summary>
        private readonly DynamicContentClient _contentHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="BannerController"/> class.
		/// </summary>
		/// <param name="contentHelper">The content helper.</param>
        public BannerController(DynamicContentClient contentHelper)
        {
            _contentHelper = contentHelper;
        }

		/// <summary>
		/// Shows the dynamic content
		/// </summary>
		/// <param name="placeName">Name of dynamic content place.</param>
		/// <returns>ActionResult.</returns>
		//[CustomOutputCache(CacheProfile = "BannerCache", VaryByCustom = "store")]
        public ActionResult ShowDynamicContent(string placeName)
        {
            var items = _contentHelper.GetDynamicContent(placeName);
            if (items != null && items.Any())
            {
                return View("BaseContentPlace", new BannerModel(items));
            }
            return null;
        }
    }
}