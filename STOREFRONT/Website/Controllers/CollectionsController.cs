#region
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("collections")]
    public class CollectionsController : BaseController
    {
        #region Public Methods and Operators
        [Route("all/{tags?}")]
        public async Task<ActionResult> AllAsync(
            string tags,
            string view = "grid",
            int page = 1,
            string sort_by = "manual")
        {
            this.Context.Set("Collection", await this.Service.GetAllCollectionAsync(sort_by));
            this.Context.Set("current_page", page);
            this.Context.Set("current_tags", this.ParseTags(tags));

            var template = "collection";
            if (view == "list")
            {
                template += ".list";
            }

            return View(template);
        }

        [Route("{category}/{tags?}", Order = 1)]
        public async Task<ActionResult> GetCollectionAsync(
            string category,
            string tags,
            string view = "grid",
            int page = 1,
            string sort_by = "manual")
        {
            this.Context.Set("Collection", await this.Service.GetCollectionAsync(category, sort_by));
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
            this.Context.Set("Collection", await this.Service.GetCollectionByKeywordAsync(category, sort_by));
            this.Context.Set("current_page", page);
            this.Context.Set("current_tags", this.ParseTags(tags));

            var template = "collection";
            if (view == "list")
            {
                template += ".list";
            }

            return View(template);
        }

        [Route("{tags?}", Order = 2)]
        public async Task<ActionResult> IndexAsync(string tags, int page = 1, string sort_by = "manual")
        {
            this.Context.Set("current_tags", this.ParseTags(tags));
            return this.View("list-collections");
        }
        #endregion

        #region Methods
        private string[] ParseTags(string tags)
        {
            if (String.IsNullOrEmpty(tags))
            {
                return null;
            }

            var tagsArray = tags.Split(new[] { '+' });
            return tagsArray;
        }
        #endregion
    }
}