using System;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Storefront.Models;

namespace VirtoCommerce.Storefront.Routing
{
    #region Temporary declarations

    public class UrlRecord
    {
        public int EntityId { get; set; }
        public string EntityType { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }
    }

    public interface IUrlRecordService
    {
        UrlRecord GetBySlug(string slug);
        string GetActiveSlug(string entityType, int entityId, string language);
    }

    #endregion

    public class SeoRoute : LocalizedRoute
    {
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;

        public SeoRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
            // TODO: initialize dependencies
            _urlRecordService = null;
            _workContext = null;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);

            if (data != null)
            {
                var slug = data.Values["generic_se_name"] as string;
                var urlRecord = _urlRecordService.GetBySlug(slug);

                if (urlRecord == null)
                {
                    // Slug not found
                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                }
                else
                {
                    // Ensure the slug is active
                    if (!urlRecord.IsActive)
                    {
                        // Slug is not active. Try to find the active one for the same entity.
                        var activeSlug = _urlRecordService.GetActiveSlug(urlRecord.EntityType, urlRecord.EntityId, urlRecord.Language);

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
                            response.RedirectLocation = string.Format("{0}{1}", _workContext.GetStoreUrl(false), activeSlug);
                            response.End();
                            data = null;
                        }
                    }
                    else
                    {
                        // Redirect to the slug for current language if it differes from requested slug
                        var slugForCurrentLanguage = GetSlug(urlRecord.EntityType, urlRecord.EntityId, _workContext.WorkingLanguage);

                        if (!string.IsNullOrEmpty(slugForCurrentLanguage) && !slugForCurrentLanguage.Equals(slug, StringComparison.OrdinalIgnoreCase))
                        {
                            var response = httpContext.Response;
                            response.Status = "302 Moved Temporarily";
                            response.RedirectLocation = string.Format("{0}{1}", _workContext.GetStoreUrl(false), slugForCurrentLanguage);
                            response.End();
                            data = null;
                        }
                        else
                        {
                            // Process the URL
                            switch (urlRecord.EntityType)
                            {
                                case "Product":
                                    data.Values["controller"] = "Product";
                                    data.Values["action"] = "ProductDetails";
                                    data.Values["productid"] = urlRecord.EntityId;
                                    data.Values["SeName"] = urlRecord.Slug;
                                    break;
                                case "Category":
                                    data.Values["controller"] = "Catalog";
                                    data.Values["action"] = "Category";
                                    data.Values["categoryid"] = urlRecord.EntityId;
                                    data.Values["SeName"] = urlRecord.Slug;
                                    break;
                            }
                        }
                    }
                }
            }

            return data;
        }

        private string GetSlug(string entityType, int entityId, string language)
        {
            var result = string.Empty;

            // Get slug for requested language
            if (!string.IsNullOrEmpty(language) && _workContext.StoreLanguages.Length >= 2)
            {
                result = _urlRecordService.GetActiveSlug(entityType, entityId, language);
            }

            // Get slug for default language
            if (string.IsNullOrEmpty(result))
            {
                result = _urlRecordService.GetActiveSlug(entityType, entityId, null);
            }

            return result;
        }
    }
}
