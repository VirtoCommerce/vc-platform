using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            if (page is BlogArticle)
            {
                base.WorkContext.CurrentBlogArticle = page as BlogArticle;
                return View("article", page.Layout, base.WorkContext);
            }
            else
            {
                base.WorkContext.CurrentPage = page as ContentPage;
                return View("page", page.Layout, base.WorkContext);
            }
        }

        // GET: /pages/{page}
        public ActionResult GetContentPageByName(string page)
        {

            var contentPages = base.WorkContext.Pages.Where(x => string.Equals(x.Url, page, StringComparison.OrdinalIgnoreCase));
            var contentPage = contentPages.FirstOrDefault(x => x.Language == base.WorkContext.CurrentLanguage);
            if(contentPage == null)
            {
                contentPage = contentPages.FirstOrDefault(x => x.Language.IsInvariant);
            }

            if (contentPage != null)
            {
                base.WorkContext.CurrentPage = contentPage as ContentPage;

                return View("page", base.WorkContext);
            }
            throw new HttpException(404, "Page with " + page + " not found.");
        }
    }
}