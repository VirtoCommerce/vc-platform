using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Tagging;

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
            var collection = new Collection() { Id = "All", SortBy = sort_by };

            Context.Set("Collection", collection);
            Context.Set("current_page", page);
            Context.Set("current_tags", ParseTags(tags));

            var template = await Task.FromResult("collection");
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
            Context.Set("Collection", await Service.GetCollectionAsync(SiteContext.Current, category));
            Context.Set("current_page", page);

            var currentTags = ParseTags(tags);
            if (currentTags == null)
                currentTags = ParseTags(constraint, ' ');

            Context.Set("current_tags", currentTags);

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
            var categoryModel = await Service.GetCollectionByKeywordAsync(SiteContext.Current, category, sort_by) ?? await Service.GetCollectionAsync(SiteContext.Current, category, sort_by);

            if (categoryModel != null)
            {
                var keyword = categoryModel.Keywords.SeoKeyword();
                SetPageMeta(keyword);
            }

            Context.Set("Collection", categoryModel);
            Context.Set("current_page", page);

            var currentTags = ParseTags(tags);
            if (currentTags == null)
                currentTags = ParseTags(constraint, ' ');

            Context.Set("current_tags", currentTags);

            var template = "collection";
            if (!string.IsNullOrEmpty(view) && view.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                template = string.Format("{0}.{1}", template, view);
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
