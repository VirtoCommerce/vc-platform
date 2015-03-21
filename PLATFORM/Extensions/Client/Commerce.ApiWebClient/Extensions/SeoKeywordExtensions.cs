using Microsoft.Practices.ServiceLocation;
using System;
using System.Globalization;
using System.Linq;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Session;

    public static class SeoKeywordExtensions
    {

        /// <summary>
        /// Gets the customer session service.
        /// </summary>
        /// <value>The customer session service.</value>
        public static ICustomerSession CustomerSession
        {
            get { return ClientContext.Session; }
        }

        /// <summary>
        /// Get correct seo keyword by language from list of given keywords
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static SeoKeyword SeoKeyword(this SeoKeyword[] keywords, string language = null)
        {
            if (keywords == null || !keywords.Any())
            {
                return null;
            }

            language = language ?? CustomerSession.Language;
            var langInfo = TryGetCultureInfo(language);
            language = langInfo != null ? langInfo.Name : language;

            //Filter keywords with valid language
            keywords = keywords.Where(x => TryGetCultureInfo(x.Language) != null).ToArray();

            if (keywords.Length != 0)
            {
                var seoKeyword =
                    keywords.FirstOrDefault(x => x.Language.Equals(language, StringComparison.InvariantCultureIgnoreCase));

                if (seoKeyword != null)
                {
                    return seoKeyword;
                }

                 //Default language failover scenario
                var store = StoreHelper.StoreClient.GetCurrentStore();

                //Current store can be null when called from StoreHttpModule and store is not yet initialzed
                if (store == null && keywords[0].KeywordType == SeoUrlKeywordTypes.Store)
                {
                    store = StoreHelper.StoreClient.GetStore(keywords[0].KeywordValue);
                }

                if (store != null && !store.DefaultLanguage.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    return keywords.FirstOrDefault(x => x.Language.Equals(store.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return null;
        }

        private static CultureInfo TryGetCultureInfo(string languageCode)
        {
            try
            {

                return !string.IsNullOrEmpty(languageCode) ? CultureInfo.CreateSpecificCulture(languageCode) : null;
            }
            catch
            {
                return null;
            }

        }
    }
}
