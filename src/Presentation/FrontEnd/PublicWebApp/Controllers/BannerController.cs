using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.ApiWebClient.Clients;

namespace VirtoCommerce.Web.Controllers
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;

    /// <summary>
	/// Class BannerController.
	/// </summary>
	[RoutePrefix("dynamic")]
    public class BannerController : ControllerBase
    {
	    public BannerController()
	    {
	        
	    }

		/// <summary>
		/// Shows the dynamic content
		/// </summary>
		/// <param name="placeName">Name of dynamic content place.</param>
		/// <returns>ActionResult.</returns>
        //[DonutOutputCache(CacheProfile = "BannerCache")]
        [Route("content")]
        [ChildActionOnly]
        public ActionResult ShowDynamicContent(string placeName)
		{
		    var session = StoreHelper.CustomerSession;
            var client = ClientContext.Clients.CreateDefaultContentClient();
            var result = Task.Run(() => client.GetDynamicContentAsync(new [] { placeName }, session.GetCustomerTagSet())).GetAwaiter().GetResult();
            if (result != null && result.TotalCount > 0)
            {
                return PartialView("BaseContentPlace", new BannerModel(result.Items.First().Items.ToArray()));
            }
            return null;
        }

        //[DonutOutputCache(CacheProfile = "BannerCache")]
         [Route("contents")]
        public async Task<ActionResult> ShowDynamicContents(string[] placeName)
        {
            var session = StoreHelper.CustomerSession;
            var client = ClientContext.Clients.CreateDefaultContentClient();
            var result = await client.GetDynamicContentAsync(placeName, session.GetCustomerTagSet());
            if (result != null && result.TotalCount > 0)
            {
                return PartialView("MultiBanner", result.Items);
            }
            return null;
        }
    }
}