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
            Func<string, ContentItem> blogsFactory = contentUrl =>
            {
                if(contentUrl.EndsWith("default"))
                {
                    return new Blog(blog, context.CurrentLanguage);
                }
                return new BlogArticle(contentUrl, context.CurrentLanguage);
            };

            var blogContents = _contentService.LoadContentItemsByUrl("/blogs/" + blog, context.CurrentStore, context.CurrentLanguage,
                                                                     blogsFactory, context.CurrentBlogSearchCritera.PageNumber, context.CurrentBlogSearchCritera.PageSize + 1);
            var contentBlog = blogContents.OfType<Blog>().FirstOrDefault();
            if (contentBlog != null)
            {
                contentBlog.Articles = new StorefrontPagedList<BlogArticle>(blogContents.OfType<BlogArticle>(), context.CurrentBlogSearchCritera.PageNumber, context.CurrentBlogSearchCritera.PageSize - 1, blogContents.TotalItemCount - 1, page => context.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());
                context.CurrentBlog = contentBlog;
                return View("blog", base.WorkContext);
            }
            throw new HttpException(404, blog);
        }

        // GET: /blogs/{blog}/{article}
        public ActionResult GetBlogArticle(string blog, string article)
        {
            var context = base.WorkContext;
            var url = String.Join("/", "/blogs", blog, article);
            var blogArticle = _contentService.LoadContentItemsByUrl(url, context.CurrentStore, context.CurrentLanguage, x => new BlogArticle(x, context.CurrentLanguage)).FirstOrDefault();
            if (blogArticle != null)
            {

                base.WorkContext.CurrentBlogArticle = blogArticle as BlogArticle;
                return View("article", base.WorkContext);

            }
            throw new HttpException(404, url);
        }
    }
}