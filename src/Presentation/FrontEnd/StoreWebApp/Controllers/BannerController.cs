using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class BannerController.
	/// </summary>
	public class BannerController : ControllerBase
    {
		/// <summary>
		/// The _content helper
		/// </summary>
        private readonly DynamicContentClient _contentHelper;

	    public BannerController()
	    {
	        
	    }

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
        //[DonutOutputCache(CacheProfile = "BannerCache")]
        public ActionResult ShowDynamicContent(string placeName)
        {
            var items = _contentHelper.GetDynamicContent(placeName);
            if (items != null && items.Any())
            {
                return PartialView("BaseContentPlace", new BannerModel(items));
            }
            return null;
        }

        //[DonutOutputCache(CacheProfile = "BannerCache")]
        public ActionResult ShowDynamicContents(string[] placeName)
        {
            return PartialView("MultiBanner", placeName);
        }
    }
}