using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Routing
{
    public class SeoRoute : Route
    {
        private readonly Func<WorkContext> _workContextFactory;
        private readonly ICommerceCoreModuleApi _commerceCoreApi;

        public SeoRoute(string url, IRouteHandler routeHandler, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi)
            : base(url, routeHandler)
        {
            _workContextFactory = workContextFactory;
            _commerceCoreApi = commerceCoreApi;
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);

            if (data != null)
            {
                var path = data.Values["path"] as string;
                var seoRecords = GetSeoRecords(path);
                var seoRecord = seoRecords.FirstOrDefault();

                if (seoRecord == null)
                {
                    // Slug not found
                    data.Values["controller"] = "Error";
                    data.Values["action"] = "Http404";
                }
                else
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
                                    workContext.CurrentCatalogSearchCriteria.CategoryId = seoRecord.ObjectId;
                                    data.Values["controller"] = "CatalogSearch";
                                    data.Values["action"] = "SearchProducts";
                                    data.Values["searchCriteria"] = workContext.CurrentCatalogSearchCriteria;

                                    break;
                            }
                        }
                    }
                }
            }

            return data;
        }


        private List<VirtoCommerceDomainCommerceModelSeoInfo> GetSeoRecords(string path)
        {
            var seoRecords = new List<VirtoCommerceDomainCommerceModelSeoInfo>();

            if (path != null)
            {
                var tokens = path.Split('/');
                // TODO: Store path tokens as breadcrumbs to the work context
                var slug = tokens.LastOrDefault();
                seoRecords = _commerceCoreApi.CommerceGetSeoInfoBySlug(slug);
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
