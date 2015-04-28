using DotLiquid;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers
{
    public class DynamicContentController : BaseController
    {
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
                            placeholder.Items.Add(HttpUtility.HtmlEncode(Parse(item)));
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
    }
}