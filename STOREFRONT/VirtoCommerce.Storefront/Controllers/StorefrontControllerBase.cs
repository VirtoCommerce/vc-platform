using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    public class StorefrontControllerBase : Controller
    {
        public StorefrontControllerBase(WorkContext context, IStorefrontUrlBuilder urlBuilder)
        {
            WorkContext = context;
            UrlBuilder = urlBuilder;
        }

        protected IStorefrontUrlBuilder UrlBuilder { get; private set; }
        protected WorkContext WorkContext { get; private set; }


        protected RedirectResult StoreFrontRedirect(string url)
        {
            var newUrl = Url.IsLocalUrl(url) ? url : "~/";
            var appRelativeUrl = UrlBuilder.ToAppRelative(WorkContext, newUrl, WorkContext.CurrentStore, WorkContext.CurrentLanguage);
            return base.Redirect(appRelativeUrl);
        }

    }
}