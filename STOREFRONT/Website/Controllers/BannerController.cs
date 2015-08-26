using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Banners;
using VirtoCommerce.Web.Convertors;
using System.Collections.Generic;

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
            var response = await Service.GetDynamicContentAsync(Context.StoreId, new[] { placeName });
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
            var response = await Service.GetDynamicContentAsync(Context.StoreId, placeNames);
            if (response != null && response.Items != null)
            {
                var placeholders = new List<PlaceHolder>();

                foreach (var item in response.Items)
                {
                    placeholders.Add(item.AsWebModel());
                }

                Context.Set("placeholders", new PlaceHolderCollection(placeholders));

                return PartialView("placeholders", this.Context);
            }

            return null;
        }
    }
}