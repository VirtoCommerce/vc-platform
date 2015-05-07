using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;
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
            if (response != null)
            {
                //Context.Set("banner", response.Items.First().Items.ToArray().First().AsWebModel());
                return PartialView("banner", this.Context);
            }
            
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> ShowDynamicContents(string[] placeNames)
        {
            // Only one placeholder can be requested on service for now.
            // Will be fixed.

            var placeholders = new List<PlaceHolder>();

            foreach (var placeName in placeNames)
            {
                var response = await Service.GetDynamicContentAsync(new[] { placeName });
                if (response != null)
                {
                    var banners = new List<Banner>();

                    foreach (var contentItem in response)
                    {
                        var banner = contentItem.AsWebModel();

                        IDictionary<string, string> bannerAdditionalProperties = null;

                        if (contentItem.ContentType == "ProductWithImageAndPrice")
                        {
                            bannerAdditionalProperties = await GetProductBannerInfoAsync(banner.Properties["productCode"]);
                        }
                        if (contentItem.ContentType == "CategoryWithImages")
                        {
                            bannerAdditionalProperties = await GetCategoryBannerInfoAsync(banner.Properties["categoryId"]);
                        }

                        if (bannerAdditionalProperties != null)
                        {
                            foreach (var property in bannerAdditionalProperties)
                            {
                                banner.Properties.Add(property);
                            }
                        }

                        banners.Add(banner);
                    }

                    placeholders.Add(new PlaceHolder
                    {
                        Name = placeName,
                        Banners = new BannerCollection(banners)
                    });
                }
            }

            Context.Set("placeholders", new PlaceHolderCollection(placeholders));

            return PartialView("placeholders", this.Context);
        }

        private async Task<IDictionary<string, string>> GetProductBannerInfoAsync(string handle)
        {
            Dictionary<string, string> info = null;

            var product = await Service.GetProductAsync(handle);

            if (product != null)
            {
                info = new Dictionary<string, string>();

                info.Add("productName", product.Title);
                info.Add("productImage", product.FeaturedImage.Src);
                info.Add("productPrice", product.Price.ToString("#.00", CultureInfo.GetCultureInfo("en-US")));
                info.Add("productUrl", product.Url);
            }

            return info;
        }

        private async Task<IDictionary<string, string>> GetCategoryBannerInfoAsync(string handle)
        {
            Dictionary<string, string> info = null;

            var collection = await Service.GetCollectionByKeywordAsync(handle);

            if (collection != null)
            {
                info = new Dictionary<string, string>();

                info.Add("categoryName", collection.Title);
                info.Add("categoryUrl", collection.Url);
            }

            return info;
        }
    }
}