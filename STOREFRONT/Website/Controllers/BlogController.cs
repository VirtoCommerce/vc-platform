#region

using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Cms;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("blogs")]
    public class BlogController : StoreControllerBase
    {
        #region Public Methods and Operators
        [Route("{blog}")]
        public async Task<ActionResult> DisplayBlogAsync(string blog)
        {
            var context = SiteContext.Current;
            var model = context.Blogs[blog];
            if (model == null)
                throw new HttpException(404, "NotFound");

            Context.Set("blog", model);

            return View("blog");
        }

        [Route("{blog}/{handle}")]
        public async Task<ActionResult> DisplayBlogArticleAsync(string blog, string handle)
        {
            var context = SiteContext.Current;
            var blogModel = context.Blogs[blog] as Blog;
            if (blogModel == null)
                throw new HttpException(404, "NotFound");

            var articleModel = blogModel.Articles.SingleOrDefault(x => x.Handle.Equals(handle));
            Context.Set("blog", blogModel);
            Context.Set("article", articleModel);

            return View("article");
        }
        #endregion
    }
}