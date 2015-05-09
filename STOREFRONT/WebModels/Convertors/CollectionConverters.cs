using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models.Extensions;
using Data = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class CollectionConverters
    {
        public static Collection AsWebModel(this Data.Category category)
        {
            var collection = new Collection();

            var urlTemplate = VirtualPathUtility.ToAbsolute("~/collections/{0}");

            collection.AllTags = null; // TODO
            collection.AllTypes = null; // TODO
            collection.AllVendors = null; // TODO
            collection.CurrentType = null; // TODO
            collection.CurrentVendor = null; // TODO
            collection.DefaultSortBy = "manual";
            collection.Description = null; // TODO
            collection.Handle = category.Code;
            collection.Id = category.Id;
            collection.Image = null; // TODO
            collection.Keywords = category.Seo != null ? category.Seo.Select(k => k.AsWebModel()) : null;
            collection.NextProduct = null; // TODO
            collection.Parents = category.Parents != null ? category.Parents.Select(p => p.AsWebModel()) : null;
            collection.PreviousProduct = null; // TODO
            collection.TemplateSuffix = null; // TODO
            collection.Title = category.Name;
            collection.Url = string.Format(urlTemplate, category.Code);

            // specify SEO based url
            var outline = collection.BuildOutline(Thread.CurrentThread.CurrentUICulture.Name).Select(x => x.Value);
            if (outline.Any())
            {
                var urlHelper = GetUrlHelper();
                collection.Outline = string.Join("/", outline);
                collection.Url = urlHelper.CategoryUrl(collection.Outline);
            }

            return collection;
        }

        private static UrlHelper GetUrlHelper()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return null;
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);

            var urlHelper = new UrlHelper(requestContext);
            return urlHelper;

        }
    }
}