﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
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

        [CLSCompliant(false)]
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
                var path = data.Values["path"] as string;
                var seoRecords = GetSeoRecords(path);
                var seoRecord = seoRecords.FirstOrDefault();
             
                if(seoRecord != null)
                {
                    var workContext = _workContextFactory();

                    // Ensure the slug is active
                    if (seoRecord.IsActive == null || !seoRecord.IsActive.Value)
                    {
                        // Slug is not active. Try to find the active one for the same entity and language.
                        var activeSlug = FindActiveSlug(seoRecords, seoRecord.ObjectType, seoRecord.ObjectId, seoRecord.LanguageCode);

                        if (string.IsNullOrWhiteSpace(activeSlug))
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
                            response.RedirectLocation = string.Format("{0}{1}", workContext.CurrentStore.Url, activeSlug);
                            response.End();
                            data = null;
                        }
                    }
                    else
                    {
                        // Redirect to the slug for the current language if it differs from the requested slug
                        var slugForCurrentLanguage = GetSlug(seoRecords, workContext, seoRecord.ObjectType, seoRecord.ObjectId, workContext.CurrentLanguage.CultureName);

                        if (!string.IsNullOrEmpty(slugForCurrentLanguage) && !slugForCurrentLanguage.Equals(seoRecord.SemanticUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            var response = httpContext.Response;
                            response.Status = "302 Moved Temporarily";
                            response.RedirectLocation = string.Format("{0}{1}", workContext.CurrentStore.Url, slugForCurrentLanguage);
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
                    var workContext = _workContextFactory();
                    var contentPage = TryToFindContentPageWithUrl(path, workContext.CurrentStore, workContext.CurrentLanguage);
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

        private ContentPage TryToFindContentPageWithUrl(string url, Store store, Language language)
        {
            if (store == null)
                return null;
            var cacheKey = String.Join(":", "TryToFindContentPageWithUrl", url, store.Id, language.CultureName);
            var retVal = _cacheManager.Get(cacheKey, "ContentRegion", () =>
            {
                var allPages = _contentService.LoadContentItemsByUrl("/", store, language, x => new ContentPage(x, language), 1, int.MaxValue);
                return allPages.FirstOrDefault(x => url.EndsWith(x.Url)) as ContentPage;
            });
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
                    seoRecords = _cacheManager.Get("CommerceGetSeoInfoBySlug-" + slug, "ApiRegion", () => { return _commerceCoreApi.CommerceGetSeoInfoBySlug(slug); });
                }
            }

            return seoRecords;
        }

        private string GetSlug(List<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords, WorkContext workContext, string entityType, string entityId, string language)
        {
            var result = string.Empty;

            // Get slug for requested language
            if (!string.IsNullOrEmpty(language) && workContext.CurrentStore.Languages.Count >= 2)
            {
                result = FindActiveSlug(seoRecords, entityType, entityId, language);
            }

            // Get slug for default language
            if (string.IsNullOrEmpty(result))
            {
                result = FindActiveSlug(seoRecords, entityType, entityId, null);
            }

            return result;
        }

        private string FindActiveSlug(List<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords, string entityType, string entityId, string language)
        {
            return seoRecords
                .Where(r => r.ObjectType == entityType && r.ObjectId == entityId && string.Equals(r.LanguageCode, language, StringComparison.OrdinalIgnoreCase) && r.IsActive != null && r.IsActive.Value)
                .Select(r => r.SemanticUrl)
                .FirstOrDefault();
        }
    }
}
