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
        private readonly WorkContext _workContext;

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
                //performance optimization.
                //we load a cached verion here. it reduces number of SQL requests for each page load
                var urlRecord = _urlRecordService.GetBySlug(slug);
                if (urlRecord == null)
                {
                    //no URL record found

                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                    return data;
                }

                //ensre that URL record is active
                if (!urlRecord.IsActive)
                {
                    //URL record is not active. let's find the latest one
                    var activeSlug = _urlRecordService.GetActiveSlug(urlRecord.EntityType, urlRecord.EntityId, urlRecord.Language);
                    if (string.IsNullOrWhiteSpace(activeSlug))
                    {
                        //no active slug found

                        data.Values["controller"] = "Common";
                        data.Values["action"] = "PageNotFound";
                        return data;
                    }

                    //the active one is found
                    var response = httpContext.Response;
                    response.Status = "301 Moved Permanently";
                    response.RedirectLocation = string.Format("{0}{1}", _workContext.CurrentStore.Url, activeSlug);
                    response.End();
                    return null;
                }

                // Redirect to the slug for current language if it differes from requested slug
                var slugForCurrentLanguage = GetSlug(urlRecord.EntityType, urlRecord.EntityId, _workContext.CurrentLanguage, true, true);
                if (!string.IsNullOrEmpty(slugForCurrentLanguage) && !slugForCurrentLanguage.Equals(slug, StringComparison.OrdinalIgnoreCase))
                {
                    var response = httpContext.Response;
                    response.Status = "302 Moved Temporarily";
                    response.RedirectLocation = string.Format("{0}{1}", _workContext.CurrentStore.Url, slugForCurrentLanguage);
                    response.End();
                    return null;
                }

                //process URL
                switch (urlRecord.EntityType)
                {
                    case "Product":
                        {
                            data.Values["controller"] = "Product";
                            data.Values["action"] = "ProductDetails";
                            data.Values["productid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "Category":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Category";
                            data.Values["categoryid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                }
            }

            return data;
        }

        public string GetSlug(string entityType, int entityId, string language, bool returnDefaultValue, bool ensureTwoPublishedLanguages)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(language)
                && (!ensureTwoPublishedLanguages || _workContext.CurrentLanguage.Length >= 2)
                )
            {
                result = _urlRecordService.GetActiveSlug(entityType, entityId, language);
            }

            // Set default value if required
            if (string.IsNullOrEmpty(result) && returnDefaultValue)
            {
                result = _urlRecordService.GetActiveSlug(entityType, entityId, null);
            }

            return result;
        }
    }
}
