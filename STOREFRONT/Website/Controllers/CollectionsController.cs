#region
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Tagging;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("collections")]
    public class CollectionsController : StoreControllerBase
    {
        #region Public Methods and Operators
        [Route("all/{tags?}")]
        public async Task<ActionResult> AllAsync(
            string tags,
            string view = "",
            int page = 1,
            string sort_by = "manual")
        {
            var collection = new Collection() { Id = "All", SortBy = sort_by  };

            this.Context.Set("Collection", collection);
            this.Context.Set("current_page", page);
            this.Context.Set("current_tags", this.ParseTags(tags));

            var template = "collection";
            if (!string.IsNullOrEmpty(view))
            {
                template = string.Format("{0}.{1}", template, view);
            }

            return View(template);
        }

        //[Route("{category}/{tags?}", Order = 1)]
        public async Task<ActionResult> GetCollectionAsync(
            string category,
            string tags,
            string view = "",
            int page = 1,
            string sort_by = "manual",
            string constraint = "")
        {
            this.Context.Set("Collection", await this.Service.GetCollectionAsync(SiteContext.Current, category));
            this.Context.Set("current_page", page);

            var currentTags = this.ParseTags(tags);
            if(currentTags == null)
                currentTags = this.ParseTags(constraint, ' ');

            this.Context.Set("current_tags", currentTags);

            var template = "collection";
            if (!string.IsNullOrEmpty(view))
            {
                template = string.Format("{0}.{1}", template, view);
            }

            return View(template);
        }

        public async Task<ActionResult> GetCollectionByKeywordAsync(
            string category,
            string tags,
            string view = "",
            int page = 1,
            string sort_by = "manual",
            string constraint = "")
        {
            var categoryModel = await this.Service.GetCollectionByKeywordAsync(SiteContext.Current, category, sort_by) ?? await this.Service.GetCollectionAsync(SiteContext.Current, category, sort_by);

            if (categoryModel != null)
            {
                var keyword = categoryModel.Keywords.SeoKeyword();
                SetPageMeta(keyword);
            }

            this.Context.Set("Collection", categoryModel);
            this.Context.Set("current_page", page);

            var currentTags = this.ParseTags(tags);
            if (currentTags == null)
                currentTags = this.ParseTags(constraint, ' ');

            this.Context.Set("current_tags", currentTags);

            var template = "collection";
            if (!string.IsNullOrEmpty(view) && view.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                template = String.Format("{0}.{1}", template, view);
            }

            return View(template);
        }

        [Route("", Order = 2)]
        public async Task<ActionResult> IndexAsync(string tags, int page = 1, string sort_by = "manual")
        {
            var result = await Task.FromResult("list-collections");
            return View(result);
        }
        #endregion

        #region Methods
        private SelectedTagCollection ParseTags(string tags, char splitter = ',')
        {
            if (string.IsNullOrEmpty(tags))
            {
                return null;
            }

            var tagsArray = tags.Split(new[] { splitter });

            // convert values to more appropriate to display values

            /*
            if (allTags != null)
            {
                for (int index = 0; index < tagsArray.Length; index++)
                {
                    var val = tagsArray[index];
                    foreach (var item in allTags.Root)
                    {
                        if (item.Equals(val))
                        {
                            val = item.Label;
                        }
                    }
                    tagsArray[index] = val;
                }
            }
            */

            return new SelectedTagCollection(tagsArray);
        }
        #endregion
    }
}