#region
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;

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
            string view = "grid",
            int page = 1,
            string sort_by = "manual")
        {
            var collections = await this.Service.GetCollectionsAsync(SiteContext.Current, sort_by);

            this.Context.Set("Collection", collections.First());
            this.Context.Set("current_page", page);
            this.Context.Set("current_tags", this.ParseTags(tags));

            var template = "collection";
            if (view == "list")
            {
                template += ".list";
            }

            return View(template);
        }

        //[Route("{category}/{tags?}", Order = 1)]
        public async Task<ActionResult> GetCollectionAsync(
            string category,
            string tags,
            string view = "grid",
            int page = 1,
            string sort_by = "manual")
        {
            this.Context.Set("Collection", await this.Service.GetCollectionAsync(SiteContext.Current, category));
            this.Context.Set("current_page", page);
            this.Context.Set("current_tags", this.ParseTags(tags));

            var template = "collection";
            if (view == "list")
            {
                template += ".list";
            }

            return View(template);
        }

        public async Task<ActionResult> GetCollectionByKeywordAsync(
            string category,
            string tags,
            string view = "grid",
            int page = 1,
            string sort_by = "manual")
        {
            var categoryModel = await this.Service.GetCollectionByKeywordAsync(SiteContext.Current, category, sort_by) ?? await this.Service.GetCollectionAsync(SiteContext.Current, category, sort_by);

            if (categoryModel != null)
            {
                var keyword = categoryModel.Keywords.SeoKeyword();
                SetPageMeta(keyword);
            }

            this.Context.Set("Collection", categoryModel);
            this.Context.Set("current_page", page);
            this.Context.Set("current_tags", this.ParseTags(tags));

            var template = "collection";
            if (view == "list")
            {
                template += ".list";
            }

            return View(template);
        }

        [Route("", Order = 2)]
        public async Task<ActionResult> IndexAsync(string tags, int page = 1, string sort_by = "manual")
        {
            await Task.FromResult<object>(null);
            Context.Set("current_tags", ParseTags(tags));
            return View("list-collections");
        }
        #endregion

        #region Methods
        private string[] ParseTags(string tags)
        {
            if (String.IsNullOrEmpty(tags))
            {
                return null;
            }

            var tagsArray = tags.Split(new[] { ',' });
            return tagsArray;
        }
        #endregion
    }
}