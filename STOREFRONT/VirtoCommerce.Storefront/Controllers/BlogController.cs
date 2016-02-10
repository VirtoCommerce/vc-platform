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
            var context = base.WorkContext;
            var pageNumber = context.CurrentBlogSearchCritera.PageNumber;
            var pageSize = context.CurrentBlogSearchCritera.PageSize;

            //var contentBlog = LoadBlog(blog);

            //Then need load all blog articles and exclude blog file from result
            //var blogArticles = _contentService.LoadContentItemsByUrl("/blogs/" + blog, context.CurrentStore, context.CurrentLanguage, () => new BlogArticle(), new[] { "default" }, pageNumber, pageSize);
            //contentBlog.Articles = new StorefrontPagedList<BlogArticle>(blogArticles.OfType<BlogArticle>(), blogArticles, page => context.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());

            context.CurrentBlog = context.Blogs.Where(x=>x.Name.Equals(blog, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            return View("blog", base.WorkContext);
        }

        // GET: /blogs/{blog}/{article}
        public ActionResult GetBlogArticle(string blog, string article)
        {
            var context = base.WorkContext;
            var articleUrl = string.Join("/", "/blogs", blog, article);

            context.CurrentBlog = context.Blogs.Where(x => x.Name.Equals(blog, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            var blogArticle = context.CurrentBlog.Articles.Where(x => x.Url.Equals(article)).SingleOrDefault();

             if (blogArticle != null)
            {
                context.CurrentBlogArticle = blogArticle;
                return View("article", base.WorkContext);
            }

            throw new HttpException(404, articleUrl);
        }

        //private Blog LoadBlog(string blogName)
        //{
        //    var context = base.WorkContext;
        //    var retVal = _contentService.LoadContentItemsByUrl("/blogs/" + blogName + "/default", context.CurrentStore, context.CurrentLanguage, () => new Blog())
        //                                   .OfType<Blog>().FirstOrDefault();
        //    if (retVal == null)
        //    {
        //        //If default file not found need create manually
        //        retVal = new Blog();
        //        retVal.Name = blogName;
        //        retVal.Title = blogName;
        //    }
        //    if (retVal.Title == "default")
        //    {
        //        retVal.Title = blogName;
        //    }
        //    retVal.Url = "blogs/" + blogName;
        //    return retVal;
        //}
    }
}