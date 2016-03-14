using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "StaticContentCachingProfile")]
    public class BlogController : StorefrontControllerBase
    {
        private readonly IStaticContentService _contentService;

        public BlogController(WorkContext context, IStorefrontUrlBuilder urlBuilder, IStaticContentService contentService)
            : base(context, urlBuilder)
        {
            _contentService = contentService;
        }

        // GET: /blogs/{blog}
        public ActionResult GetBlog(string blog)
        {
            base.WorkContext.CurrentBlog = base.WorkContext.Blogs.Where(x=>x.Name.Equals(blog, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            return View("blog", base.WorkContext);
        }

        // GET: /blogs/{blog}/{article}
        public ActionResult GetBlogArticle(string blog, string article)
        {
            var context = base.WorkContext;
            var articleUrl = string.Join("/", "blogs", blog, article);

            context.CurrentBlog = context.Blogs.Where(x => x.Name.Equals(blog, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            var blogArticles = context.CurrentBlog.Articles.Where(x => x.Url.Equals(articleUrl));
            //Need return article with current  or  invariant language
            var blogArticle = blogArticles.FirstOrDefault(x => x.Language == context.CurrentLanguage);
            if(blogArticle == null)
            {
                blogArticle = blogArticles.FirstOrDefault(x => x.Language.IsInvariant);
            }
             if (blogArticle != null)
            {
                context.CurrentBlogArticle = blogArticle;
                return View("article", base.WorkContext);
            }

            throw new HttpException(404, articleUrl);
        }
    }
}