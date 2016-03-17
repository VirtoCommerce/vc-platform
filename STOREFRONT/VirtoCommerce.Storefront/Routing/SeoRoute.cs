using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Routing
{
    public class SeoRoute : Route
    {
        private readonly Func<WorkContext> _workContextFactory;
        private readonly ICommerceCoreModuleApi _commerceCoreApi;
        private readonly IStaticContentService _contentService;
        private readonly ICacheManager<object> _cacheManager;

        public SeoRoute(string url, IRouteHandler routeHandler, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, IStaticContentService staticContentService, ICacheManager<object> cacheManager)
            : base(url, routeHandler)
        {
            _workContextFactory = workContextFactory;
            _commerceCoreApi = commerceCoreApi;
            _contentService = staticContentService;
            _cacheManager = cacheManager;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);

            if (data != null)
            {
                //get workContext
                var workContext = _workContextFactory();

                var path = data.Values["path"] as string;
                var store = data.Values["store"] as string;
                //Special workaround for case when url contains only slug without store (one store case)
                if(string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(store) && workContext.AllStores != null)
                {
                   //use {store} as {path} if not exist any store with name {store} 
                   path = workContext.AllStores.Any(x => string.Equals(store, x.Id, StringComparison.InvariantCultureIgnoreCase)) ? null : store;
                }
                //Get all seo records for requested slug and also all other seo records with different slug and languages but related to same object
                // GetSeoRecords('A') returns 
                // { objectType: 'Product', objectId: '1',  SemanticUrl: 'A', Language: 'en-us', active : false }
                // { objectType: 'Product', objectId: '1',  SemanticUrl: 'AA', Language: 'en-us', active : true }
                var seoRecords = GetSeoRecords(path);
                var seoRecord = seoRecords.Where(x => path.Equals(x.SemanticUrl, StringComparison.OrdinalIgnoreCase))
                                          .FindBestSeoMatch(workContext.CurrentLanguage, workContext.CurrentStore);

                if (seoRecord != null)
                {
                    // Ensure the slug is active
                    if (seoRecord.IsActive == null || !seoRecord.IsActive.Value)
                    {
                        // Slug is not active. Try to find the active one for the same entity and language.
                        seoRecord = seoRecords.Where(x=>x.ObjectType == seoRecord.ObjectType && x.ObjectId == seoRecord.ObjectId && x.IsActive != null && x.IsActive.Value)
                                              .FindBestSeoMatch(workContext.CurrentLanguage, workContext.CurrentStore);
                        
                        if (seoRecord == null)
                        {
                            // No active slug found
                            data.Values["controller"] = "Error";
                            data.Values["action"] = "Http404";
                        }
                        else
                        {
                            // The active slug is found
                            var response = httpContext.Response;
                            response.Status = "301 Moved Permanently";
                            response.RedirectLocation = string.Format("{0}{1}", workContext.CurrentStore.Url, seoRecord.SemanticUrl);
                            response.End();
                            data = null;
                        }
                    }
                    else
                    {
                        // Redirect to the slug for the current language if it differs from the requested slug
                        var actualActiveSeoRecord = seoRecords.Where(x => x.ObjectType == seoRecord.ObjectType && x.ObjectId == seoRecord.ObjectId && x.IsActive != null && x.IsActive.Value)
                                                              .FindBestSeoMatch(workContext.CurrentLanguage, workContext.CurrentStore);
                        //If actual seo different that requested need redirect 302
                        if (!string.Equals(actualActiveSeoRecord.SemanticUrl, seoRecord.SemanticUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            var response = httpContext.Response;
                            response.Status = "302 Moved Temporarily";
                            response.RedirectLocation = string.Format("{0}{1}", workContext.CurrentStore.Url, actualActiveSeoRecord.SemanticUrl);
                            response.End();
                            data = null;
                        }
                        else
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
                    }
                }
                else if(!String.IsNullOrEmpty(path))
                {
                    var contentPage = TryToFindContentPageWithUrl(workContext, path);
                    if(contentPage != null)
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

            return data;
        }

        private ContentItem TryToFindContentPageWithUrl(WorkContext workContext, string url)
        {
            url = url.TrimStart('/');
            var pages = workContext.Pages.Where(x => string.Equals(x.Permalink, url, StringComparison.CurrentCultureIgnoreCase) || string.Equals(x.Url, url, StringComparison.InvariantCultureIgnoreCase));
            //Need return page with current  or  invariant language 
            var retVal = pages.FirstOrDefault(x => x.Language == workContext.CurrentLanguage);
            if(retVal == null)
            {
                retVal = pages.FirstOrDefault(x => x.Language.IsInvariant);
            }
            return retVal;
        }

        private List<VirtoCommerceDomainCommerceModelSeoInfo> GetSeoRecords(string path)
        {
            var seoRecords = new List<VirtoCommerceDomainCommerceModelSeoInfo>();

            if (path != null)
            {
                var tokens = path.Split('/');
                // TODO: Store path tokens as breadcrumbs to the work context
                var slug = tokens.LastOrDefault();
                if (!String.IsNullOrEmpty(slug))
                {
                    seoRecords = _cacheManager.Get(string.Join(":", "CommerceGetSeoInfoBySlug", slug), "ApiRegion", () => { return _commerceCoreApi.CommerceGetSeoInfoBySlug(slug); });
                }
            }

            return seoRecords;
        }

     

    }
}
