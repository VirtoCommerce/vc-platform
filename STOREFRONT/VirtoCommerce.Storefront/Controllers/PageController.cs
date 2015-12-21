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
        public ActionResult GetContentPage(ContentPage page)
        {
            base.WorkContext.CurrentPage = page;
            return View("page", base.WorkContext);
        }

        // GET: /pages/{page}
        public ActionResult GetContentPageByName(string page)
        {
            var contentPage = _contentService.LoadContentItemsByUrl(page, base.WorkContext.CurrentStore, base.WorkContext.CurrentLanguage, x => new ContentPage(x, base.WorkContext.CurrentLanguage)).FirstOrDefault();
            if (contentPage != null)
            {
                base.WorkContext.CurrentPage = contentPage as ContentPage;

                return View("page", base.WorkContext);
            }
            throw new HttpException(404, "Page with " + page + " not found.");
        }
    }
}