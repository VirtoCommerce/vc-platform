using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Banners;
using VirtoCommerce.Web.Models.Convertors;

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("banners")]
    public class BannerController : StoreControllerBase
    {
		/// <summary>
		/// Shows the dynamic content
		/// </summary>
		/// <param name="placeName">Name of dynamic content place.</param>
		/// <returns>ActionResult.</returns>
        //[DonutOutputCache(CacheProfile = "BannerCache")]
        [Route("{placename}")]
        //[ChildActionOnly]
        public async Task<ActionResult> ShowDynamicContent(string placeName)
		{
            var response = await Service.GetDynamicContentAsync(new [] { placeName });
            if (response != null && response.Items != null)
            {
                Context.Set("banner", response.Items.First().Items.ToArray().First().AsWebModel());
                return PartialView("banner", this.Context);
            }
            
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> ShowDynamicContents(string[] placeName)
        {
            var response = await Service.GetDynamicContentAsync(placeName);
            if (response != null && response.Items != null)
            {
                Context.Set("placeholders", new PlaceHolderCollection(response.Items.AsWebModel()));
                return PartialView("placeholders", this.Context);
            }

            return null;
        }
    }
}