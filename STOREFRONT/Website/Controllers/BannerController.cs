using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Banners;
using VirtoCommerce.Web.Convertors;

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
            var response = await Service.GetDynamicContentAsync(new[] { placeName });
            if (response != null && response.Items != null)
            {
                Context.Set("placeholders", new PlaceHolderCollection(response.Items.AsWebModel()));
                return PartialView("placeholders");
            }

            return null;
        }

        //[DonutOutputCache(CacheProfile = "BannerCache")]
        [Route("")]
        public async Task<ActionResult> ShowDynamicContents(string[] placeNames)
        {
            var response = await Service.GetDynamicContentAsync(placeNames);
            if (response != null && response.Items != null)
            {
                Context.Set("placeholders", new PlaceHolderCollection(response.Items.Select(x => x.AsWebModel())));
                return PartialView("placeholders", this.Context);
            }

            return null;
        }
    }
}