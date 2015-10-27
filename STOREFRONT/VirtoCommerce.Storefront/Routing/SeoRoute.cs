using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Routing
{
    public class SeoRoute : Route
    {
        private readonly ICommerceCoreModuleApi _commerceCoreApi;

        public SeoRoute(string url, IRouteHandler routeHandler, ICommerceCoreModuleApi commerceCoreApi)
            : base(url, routeHandler)
        {
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
                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                }
                else
                {
                    var workContext = httpContext.GetOwinContext().Get<WorkContext>();

                    // Ensure the slug is active
                    if (seoRecord.IsActive == null || !seoRecord.IsActive.Value)
                    {
                        // Slug is not active. Try to find the active one for the same entity and language.
                        var activeSlug = FindActiveSlug(seoRecords, seoRecord.ObjectType, seoRecord.ObjectId, seoRecord.LanguageCode);

                        if (string.IsNullOrWhiteSpace(activeSlug))
                        {
                            // No active slug found
                            data.Values["controller"] = "Common";
                            data.Values["action"] = "PageNotFound";
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
                        var slugForCurrentLanguage = GetSlug(seoRecords, workContext, seoRecord.ObjectType, seoRecord.ObjectId, workContext.CurrentLanguage);

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
                                    data.Values["productid"] = seoRecord.ObjectId;
                                    data.Values["slug"] = seoRecord.SemanticUrl;
                                    break;
                                case "Category":
                                    data.Values["controller"] = "Catalog";
                                    data.Values["action"] = "Category";
                                    data.Values["categoryid"] = seoRecord.ObjectId;
                                    data.Values["slug"] = seoRecord.SemanticUrl;
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
