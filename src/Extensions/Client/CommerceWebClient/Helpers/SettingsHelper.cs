using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.Web.Client.Helpers
{
    /// <summary>
    /// Class SettingsHelper.
    /// </summary>
	public class SettingsHelper
	{
        /// <summary>
        /// Gets the settings client.
        /// </summary>
        /// <value>The settings client.</value>
		public static SettingsClient SettingsClient
		{
			get
			{
				return ServiceLocator.Current.GetInstance<SettingsClient>();
			}
		}

        /// <summary>
        /// Gets the seo keyword client.
        /// </summary>
        /// <value>The seo keyword client.</value>
        public static SeoKeywordClient SeoKeywordClient
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SeoKeywordClient>();
            }
        }

        /// <summary>
        /// Gets the customer session service.
        /// </summary>
        /// <value>The customer session service.</value>
        public static ICustomerSession CustomerSession
        {
            get { return ServiceLocator.Current.GetInstance<ICustomerSessionService>().CustomerSession; }
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SettingValue[][].</returns>
		public static SettingValue[] GetSettings(string name)
		{
			return SettingsClient.GetSettings(name);
		}

        /// <summary>
        /// Encodes the given value to SEO keyword.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string SeoEncode(string value, SeoUrlKeywordTypes type)
        {
            return SeoEncode(value, type,CustomerSession.Language);
        }

        public static string SeoEncode(string value, SeoUrlKeywordTypes type, string language)
        {
            return SeoKeyword(language, value, type);
        }


        /// <summary>
        /// Decodes the given SEO keyword to original value.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string SeoDecode(string keyword, SeoUrlKeywordTypes type)
        {
            return SeoDecode(keyword, type, CustomerSession.Language);
        }

        public static string SeoDecode(string keyword, SeoUrlKeywordTypes type,string language)
        {
            return SeoKeyword(language, keyword, type, false);
        }


        private static string SeoKeyword(string language, string val, SeoUrlKeywordTypes type, bool getKeyword = true)
        {
            var seoKeyword = getKeyword
                ? SeoKeywordClient.GetSeoKeyword(type, language, keywordvalue: val)
                : SeoKeywordClient.GetSeoKeyword(type, language, val);

            //If not found for given langauge try default store language
            if (seoKeyword == null)
            {
                var store = StoreHelper.StoreClient.GetCurrentStore();
                if (store != null &&
                    !store.DefaultLanguage.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    seoKeyword = getKeyword
                  ? SeoKeywordClient.GetSeoKeyword(type, store.DefaultLanguage, keywordvalue: val)
                  : SeoKeywordClient.GetSeoKeyword(type, store.DefaultLanguage, val);
                }
            }
            return seoKeyword != null ? getKeyword ? seoKeyword.Keyword : seoKeyword.KeywordValue : val;
        }

        /// <summary>
        /// Gets a value indicating whether [output cache enabled].
        /// </summary>
        /// <value><c>true</c> if [output cache enabled]; otherwise, <c>false</c>.</value>
        public static bool OutputCacheEnabled
        {
            get
            {
                var retVal = true; // if there is no such setting we assume cache enabled

                var settings = GetSettings("OutputCacheEnabled");

                if (settings != null && settings.Length > 0)
                {
                    retVal = settings.First().BooleanValue;
                }

                return retVal;
            }
        }
	}
}
