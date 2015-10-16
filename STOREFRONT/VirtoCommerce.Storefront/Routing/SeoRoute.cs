using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace VirtoCommerce.Storefront.Routing
{
    #region Temporary declarations

    public class Language
    {
        public int Id { get; set; }
    }

    public class UrlRecord
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
    }

    public interface IUrlRecordService
    {
        UrlRecord GetBySlug(string slug);
        string GetActiveSlug(int entityId, string entityName, int languageId);
    }
    public interface ILanguageService
    {
        IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0);
    }

    public interface IWebHelper
    {
        string GetStoreLocation(bool useSsl);
    }

    public interface IWorkContext
    {
        Language WorkingLanguage { get; set; }
    }

    #endregion

    public class SeoRoute : LocalizedRoute
    {
        private readonly IUrlRecordService _urlRecordService;
        private readonly ILanguageService languageService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public SeoRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
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
                    var activeSlug = _urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, urlRecord.LanguageId);
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
                    response.RedirectLocation = string.Format("{0}{1}", _webHelper.GetStoreLocation(false), activeSlug);
                    response.End();
                    return null;
                }

                //ensure that the slug is the same for the current language
                //otherwise, it can cause some issues when customers choose a new language but a slug stays the same
                var slugForCurrentLanguage = GetSlug(urlRecord.EntityId, urlRecord.EntityName, _workContext.WorkingLanguage.Id, true, true);
                if (!string.IsNullOrEmpty(slugForCurrentLanguage) &&
                    !slugForCurrentLanguage.Equals(slug, StringComparison.OrdinalIgnoreCase))
                {
                    // We should check for null or "" above because some entities does not have SeName for standard (ID=0) language (e.g. news, blog posts)
                    var response = httpContext.Response;
                    response.Status = "302 Moved Temporarily";
                    response.RedirectLocation = string.Format("{0}{1}", _webHelper.GetStoreLocation(false), slugForCurrentLanguage);
                    response.End();
                    return null;
                }

                //process URL
                switch (urlRecord.EntityName.ToLowerInvariant())
                {
                    case "product":
                        {
                            data.Values["controller"] = "Product";
                            data.Values["action"] = "ProductDetails";
                            data.Values["productid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "category":
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

        public string GetSlug(int entityId, string entityName, int languageId, bool returnDefaultValue, bool ensureTwoPublishedLanguages)
        {
            string result = string.Empty;

            if (languageId > 0)
            {
                //ensure that we have at least two published languages
                bool loadLocalizedValue = true;
                if (ensureTwoPublishedLanguages)
                {
                    var totalPublishedLanguages = languageService.GetAllLanguages().Count;
                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                //localized value
                if (loadLocalizedValue)
                {
                    result = _urlRecordService.GetActiveSlug(entityId, entityName, languageId);
                }
            }

            //set default value if required
            if (string.IsNullOrEmpty(result) && returnDefaultValue)
            {
                result = _urlRecordService.GetActiveSlug(entityId, entityName, 0);
            }

            return result;
        }
    }
}
