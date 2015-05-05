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

        //[DonutOutputCache(CacheProfile = "BannerCache")]
        [Route("")]
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

        /*


        [HttpGet]
        public async Task<ActionResult> GetBanners(string[] placeholderIds)
        {
            List<DynamicContentPlaceholder> placeholders = null;

            var response = await Service.GetDynamicContentAsync(placeholderIds);

            if (response != null && response.Items != null)
            {
                placeholders = new List<DynamicContentPlaceholder>();

                foreach (var placeholderId in placeholderIds)
                {
                    var placeholder = new DynamicContentPlaceholder
                    {
                        PlaceholderId = placeholderId
                    };

                    var group = response.Items.FirstOrDefault(i => i.GroupName == placeholderId);

                    if (group != null)
                    {
                        placeholder.Items = new List<string>();

                        foreach (var item in group.Items)
                        {
                            placeholder.Items.Add(Parse(item));
                        }
                    }

                    placeholders.Add(placeholder);
                }
            }

            return placeholders != null ? Json(placeholders.ToArray()) : null;
        }

        private string Parse(DynamicContentItem item)
        {
            string html = null;

            if (item.Properties != null)
            {
                if (item.ContentType == "ImageNonClickable")
                {
                    var template = Template.Parse(
                        "<img alt=\"{{ alternative_text }}\" src=\"{{ image_url }}\" />");
                    html = template.Render(Hash.FromAnonymousObject(new
                    {
                        alternative_text = item.Properties.ContainsKey("alternativeText") ? item.Properties["alternativeText"] : null,
                        image_url = item.Properties.ContainsKey("imageUrl") ? item.Properties["imageUrl"] : null
                    }));
                }
                else if (item.ContentType == "ImageClickable")
                {
                    var template = Template.Parse(
                        "<a href=\"{{ target_url }}\">" +
                        "<img alt=\"{{ alternative_text }}\" src=\"{{ image_url }}\" title=\"{{ title }}\" />" +
                        "</a>");
                    html = template.Render(Hash.FromAnonymousObject(new
                    {
                        alternative_text = item.Properties.ContainsKey("alternativeText") ? item.Properties["alternativeText"] : null,
                        image_url = item.Properties.ContainsKey("imageUrl") ? item.Properties["imageUrl"] : null,
                        target_url = item.Properties.ContainsKey("targetUrl") ? item.Properties["targetUrl"] : null,
                        title = item.Properties.ContainsKey("title") ? item.Properties["title"] : null
                    }));
                }
            }

            return html;
        }
         * */
    }
}