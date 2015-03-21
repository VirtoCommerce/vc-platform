using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using System.Globalization;
using System.Threading.Tasks;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiWebClient.Helpers
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.ApiClient.Session;

    /// <summary>
    /// Class SettingsHelper.
    /// </summary>
	public class SettingsHelper
	{

        /// <summary>
        /// Gets the customer session service.
        /// </summary>
        /// <value>The customer session service.</value>
        public static ICustomerSession CustomerSession
        {
            get
            {
                return ClientContext.Session;
            }
        }

        public static SeoKeyword GetKeyword(string routeValue, SeoUrlKeywordTypes type, string language = null)
        {
            routeValue = routeValue.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

            var session = CustomerSession;

            language = language ?? session.Language;
            var langInfo = TryGetCultureInfo(language);
            language = langInfo != null ? langInfo.Name : language;
            var keyword = new SeoKeyword { KeywordType = type, Keyword = routeValue, KeywordValue = routeValue, Language = language };
            var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, language);
            switch (type)
            {
                case SeoUrlKeywordTypes.Store:
                    var store = StoreHelper.StoreClient.GetStore(routeValue);
                    if (store != null && store.SeoKeywords != null)
                    {
                        keyword = store.SeoKeywords.SeoKeyword(language);

                    }
                    break;
                case SeoUrlKeywordTypes.Category:
                    var category = Task.Run(() => client.GetCategoryAsync(routeValue)).Result;
                    if (category != null)
                    {
                        keyword = category.SeoKeywords.SeoKeyword(language);
                    }
                    break;
                case SeoUrlKeywordTypes.Item:
                    var item = Task.Run(() => client.GetProductAsync(routeValue, ItemResponseGroups.ItemMedium)).Result;
                    if (item != null)
                    {
                        keyword = item.SeoKeywords.SeoKeyword(language);
                    }
                    break;
            }

            return keyword;
        }

        public static string EncodeRouteValue(string routeValue, SeoUrlKeywordTypes type, string language = null)
        {
            if (!string.IsNullOrEmpty(routeValue))
            {
                routeValue = routeValue.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Last();
                var keyword = GetKeyword(routeValue, type, language);

                var client = ClientContext.Clients.CreateBrowseClient(ClientContext.Session.StoreId, language);

                if (keyword != null)
                {
                    switch (type)
                    {
                        case SeoUrlKeywordTypes.Store:
                        case SeoUrlKeywordTypes.Item:
                            return keyword.Keyword;
                        case SeoUrlKeywordTypes.Category:
                            var category =
                                Task.Run(
                                    () =>
                                        client.GetCategoryAsync(routeValue))
                                    .Result;
                            if (category != null)
                            {
                                return string.Join("/", category.BuildOutline(language).Select(x => x.Value));
                            }
                            break;
                    }
                }
            }

            return routeValue;
        }

     
        private static CultureInfo TryGetCultureInfo(string languageCode)
        {
            try
            {

                if(!string.IsNullOrEmpty(languageCode))
                    return CultureInfo.CreateSpecificCulture(languageCode);
            }
            catch
            {
            }
            return null;
        }

	}
}
