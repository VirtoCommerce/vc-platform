using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;
using VirtoCommerce.ApiWebClient.Clients;

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
            var result = Task.Run(() => _contentHelper.GetDynamicContentAsync(placeName)).Result;
            if (result != null && result.TotalCount > 0)
            {
                return PartialView("BaseContentPlace", new BannerModel(result.Items.First().Items.ToArray()));
            }
            return null;
        }

        //[DonutOutputCache(CacheProfile = "BannerCache")]
        public async Task<ActionResult> ShowDynamicContents(string[] placeName)
        {
            var result = await _contentHelper.GetDynamicContentAsync(placeName);
            if (result != null && result.TotalCount > 0)
            {
                return PartialView("MultiBanner", result.Items);
            }
            return null;
        }
    }
}