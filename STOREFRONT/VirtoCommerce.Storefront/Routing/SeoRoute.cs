using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Routing
{
    public class SeoRoute : Route
    {
        private readonly Func<WorkContext> _workContextFactory;
        private readonly ICommerceCoreModuleApi _commerceCoreApi;
        private readonly ILocalCacheManager _cacheManager;

        public SeoRoute(string url, IRouteHandler routeHandler, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, ILocalCacheManager cacheManager)
            : base(url, routeHandler)
        {
            _workContextFactory = workContextFactory;
            _commerceCoreApi = commerceCoreApi;
            _cacheManager = cacheManager;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);
            if (data != null)
            {
                var path = data.Values["path"] as string;
                if (path != null)
                {
                    var workContext = _workContextFactory();

                    var tokens = path.Split('/');
                    // TODO: Store path tokens as breadcrumbs to the work context
                    var slug = tokens.LastOrDefault();

                    // Get all seo records for requested slug and also all other seo records with different slug and languages but related to same object
                    var seoRecords = GetSeoRecords(slug);

                    var seoRecord = seoRecords.Where(x => x.IsActive == true).GetBestMatchedSeoInfo(workContext.CurrentStore, workContext.CurrentLanguage, slug);
                    if (seoRecord != null)
                    {
                        if (seoRecord.SemanticUrl.EqualsInvariant(slug))
                        {
                            // Process the URL
                            switch (seoRecord.ObjectType)
                            {
                                case "CatalogProduct":
                                    data.Values["controller"] = "Product";
                                    data.Values["action"] = "ProductDetails";
                                    data.Values["productId"] = seoRecord.ObjectId;
                                    break;
                                case "Category":
                                    data.Values["controller"] = "CatalogSearch";
                                    data.Values["action"] = "CategoryBrowsing";
                                    data.Values["categoryId"] = seoRecord.ObjectId;
                                    break;
                            }
                        }
                        else
                        {
                            var response = httpContext.Response;
                            response.Status = "302 Moved Temporarily";
                            response.RedirectLocation = string.Concat(workContext.CurrentStore.Url, seoRecord.SemanticUrl);
                            response.End();
                            data = null;
                        }
                    }
                    else if (!string.IsNullOrEmpty(path))
                    {
                        var contentPage = TryToFindContentPageWithUrl(workContext, path);
                        if (contentPage != null)
                        {
                            data.Values["controller"] = "Page";
                            data.Values["action"] = "GetContentPage";
                            data.Values["page"] = contentPage;
                        }
                        else
                        {
                            data.Values["controller"] = "Error";
                            data.Values["action"] = "Http404";
                        }
                    }
                }
            }

            return data;
        }


        private static ContentItem TryToFindContentPageWithUrl(WorkContext workContext, string url)
        {
            ContentItem result = null;

            if (workContext.Pages != null)
            {
                url = url.TrimStart('/');
                var pages = workContext.Pages
                    .Where(x =>
                            string.Equals(x.Permalink, url, StringComparison.CurrentCultureIgnoreCase) ||
                            string.Equals(x.Url, url, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

                // Return page with current language or invariant language
                result = pages.FirstOrDefault(x => x.Language == workContext.CurrentLanguage);
                if (result == null)
                {
                    result = pages.FirstOrDefault(x => x.Language.IsInvariant);
                }
            }

            return result;
        }

        private List<VirtoCommerceDomainCommerceModelSeoInfo> GetSeoRecords(string slug)
        {
            var seoRecords = new List<VirtoCommerceDomainCommerceModelSeoInfo>();

            if (!string.IsNullOrEmpty(slug))
            {
                seoRecords = _cacheManager.Get(string.Join(":", "CommerceGetSeoInfoBySlug", slug), "ApiRegion", () => _commerceCoreApi.CommerceGetSeoInfoBySlug(slug));
            }

            return seoRecords;
        }
    }
}
