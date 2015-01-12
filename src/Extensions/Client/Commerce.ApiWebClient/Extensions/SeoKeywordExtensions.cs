using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiWebClient.Customer;
using VirtoCommerce.ApiWebClient.Customer.Services;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    public static class SeoKeywordExtensions
    {

        /// <summary>
        /// Gets the customer session service.
        /// </summary>
        /// <value>The customer session service.</value>
        public static ICustomerSession CustomerSession
        {
            get { return ServiceLocator.Current.GetInstance<ICustomerSessionService>().CustomerSession; }
        }

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
            }

            return null;
        }

        private static CultureInfo TryGetCultureInfo(string languageCode)
        {
            try
            {

                if (!string.IsNullOrEmpty(languageCode))
                    return CultureInfo.CreateSpecificCulture(languageCode);
            }
            catch
            {
            }
            return null;
        }
    }
}
