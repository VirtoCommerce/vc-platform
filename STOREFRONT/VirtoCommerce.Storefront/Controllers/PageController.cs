using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "StaticContentCachingProfile")]
    public class PageController : StorefrontControllerBase
    {
        private readonly IStaticContentService _contentService;

        public PageController(WorkContext context, IStorefrontUrlBuilder urlBuilder, IStaticContentService contentService)
            : base(context, urlBuilder)
        {
            _contentService = contentService;
        }

        //Called from SEO route by page permalink
        public ActionResult GetContentPage(ContentItem page)
        {
            var blogArticle = page as BlogArticle;
            var contentPage = page as ContentPage;
            if (blogArticle != null)
            {
                base.WorkContext.CurrentBlogArticle = blogArticle;
                base.WorkContext.CurrentBlog = base.WorkContext.Blogs.Where(x => x.Name.Equals(blogArticle.BlogName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                return View("article", page.Layout, base.WorkContext);
            }
            else
            {
                base.WorkContext.CurrentPage = contentPage;
                return View("page", page.Layout, base.WorkContext);
            }
        }

        // GET: /pages/{page}
        public ActionResult GetContentPageByName(string page)
        {

            var contentPages = base.WorkContext.Pages.Where(x => string.Equals(x.Url, page, StringComparison.OrdinalIgnoreCase));
            var contentPage = contentPages.FindWithLanguage(base.WorkContext.CurrentLanguage);        
            if (contentPage != null)
            {
                base.WorkContext.CurrentPage = contentPage as ContentPage;

                return View("page", base.WorkContext);
            }
            throw new HttpException(404, "Page with " + page + " not found.");
        }
    }
}