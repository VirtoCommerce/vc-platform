using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Models;

namespace VirtoCommerce.Storefront.Routing
{
    public class SeoRoute : LocalizedRoute
    {
        private readonly ICommerceCoreModuleApi _commerceCore;

        public SeoRoute(string url, IRouteHandler routeHandler, ICommerceCoreModuleApi commerceCore)
            : base(url, routeHandler)
        {
            _commerceCore = commerceCore;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);

            if (data != null)
            {
                var workContext = httpContext.GetOwinContext().Get<WorkContext>();

                var slug = data.Values["generic_se_name"] as string;
                var seoRecords = _commerceCore.CommerceGetSeoInfoBySlug(slug);
                var seoRecord = seoRecords.FirstOrDefault(r => r.SemanticUrl == slug);

                if (seoRecord == null)
                {
                    // Slug not found
                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                }
                else
                {
                    // Ensure the slug is active
                    if (!seoRecord.IsActive())
                    {
                        // Slug is not active. Try to find the active one for the same entity.
                        var activeSlug = FindActiveSlug(seoRecord.ObjectType, seoRecord.ObjectId, seoRecord.LanguageCode, seoRecords);

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
                        // Redirect to the slug for current language if it differes from requested slug
                        var slugForCurrentLanguage = GetSlug(workContext, seoRecord.ObjectType, seoRecord.ObjectId, workContext.CurrentLanguage, seoRecords);

                        if (!string.IsNullOrEmpty(slugForCurrentLanguage) && !slugForCurrentLanguage.Equals(slug, StringComparison.OrdinalIgnoreCase))
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
                                    data.Values["productid"] = seoRecord.ObjectType;
                                    data.Values["SeName"] = seoRecord.SemanticUrl;
                                    break;
                                case "Category":
                                    data.Values["controller"] = "Catalog";
                                    data.Values["action"] = "Category";
                                    data.Values["categoryid"] = seoRecord.ObjectId;
                                    data.Values["SeName"] = seoRecord.SemanticUrl;
                                    break;
                            }
                        }
                    }
                }
            }

            return data;
        }

        private string GetSlug(WorkContext workContext, string entityType, string entityId, string language, List<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords)
        {
            var result = string.Empty;

            // Get slug for requested language
            if (!string.IsNullOrEmpty(language) && workContext.CurrentStore.Languages.Count >= 2)
            {
                result = FindActiveSlug(entityType, entityId, language, seoRecords);
            }

            // Get slug for default language
            if (string.IsNullOrEmpty(result))
            {
                result = FindActiveSlug(entityType, entityId, null, seoRecords);
            }

            return result;
        }

        private string FindActiveSlug(string entityType, string entityId, string language, List<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords)
        {
            return seoRecords
                .Where(r => r.ObjectType == entityType && r.ObjectId == entityId && r.LanguageCode == language)
                .Select(r => r.SemanticUrl)
                .FirstOrDefault();
        }
    }

    internal static class VirtoCommerceDomainCommerceModelSeoInfoExtensions
    {
        public static bool IsActive(this VirtoCommerceDomainCommerceModelSeoInfo seo)
        {
            return true;
        }
    }
}
