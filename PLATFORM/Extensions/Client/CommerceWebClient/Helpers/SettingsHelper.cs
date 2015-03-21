using System;
using System.CodeDom.Compiler;
using System.Globalization;
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
        /// <param name="language">The language.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string SeoEncode(string value, SeoUrlKeywordTypes type, string language = null, char seperator = '/')
        {
            var encoded = value.Split(new[] { seperator }).Select(x => SeoEncodeInternal(x, type, language)).ToList();
            return string.Join(seperator.ToString(CultureInfo.InvariantCulture), encoded);
        }

        private static string SeoEncodeInternal(string value, SeoUrlKeywordTypes type, string language = null)
        {
            var seoKeyword = SeoKeyword(value, type, language);
            return seoKeyword != null ? seoKeyword.Keyword : value;
        }

        /// <summary>
        /// Decodes the given SEO keyword to original value.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="type">The type.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static string SeoDecode(string keyword, SeoUrlKeywordTypes type, string language = null, char seperator = '/')
        {
            var decoded = keyword.Split(new[] { seperator }).Select(x => SeoDecodeInternal(x, type, language)).ToList();
            return string.Join(seperator.ToString(CultureInfo.InvariantCulture), decoded);
        }

        private static string SeoDecodeInternal(string keyword, SeoUrlKeywordTypes type, string language = null)
        {
            var seoKeyword = SeoKeyword(keyword, type, language, false);
            return seoKeyword != null ? seoKeyword.KeywordValue : keyword;
        }


        public static SeoUrlKeyword SeoKeyword(string val, SeoUrlKeywordTypes type, string language = null, bool byValue = true)
        {
            language = language ?? CustomerSession.Language;
            var langInfo = TryGetCultureInfo(language);
            language = langInfo != null ? langInfo.Name : language;

            var seoKeywords = byValue
                ? SeoKeywordClient.GetSeoKeywords(type, keywordvalue: val)
                : SeoKeywordClient.GetSeoKeywords(type, keyword:val);

            //Filter keywords with valid language
            seoKeywords = seoKeywords.Where(x => TryGetCultureInfo(x.Language) != null).ToArray();

            //If not found for given langauge try default store language
            if (seoKeywords.Length != 0)
            {
                var seoKeyword = seoKeywords.FirstOrDefault(x => x.Language.Equals(language, StringComparison.InvariantCultureIgnoreCase));

                if (seoKeyword != null)
                {
                    return seoKeyword;
                }

                //Default language failover scenario
                var store = StoreHelper.StoreClient.GetCurrentStore();

                //Current store can be null when called from StoreHttpModule and store is not yet initialzed
                if (store == null && type == SeoUrlKeywordTypes.Store)
                {
                    store = StoreHelper.StoreClient.GetStoreById(seoKeywords[0].KeywordValue);
                }

                if (store != null && !store.DefaultLanguage.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    seoKeyword = seoKeywords.FirstOrDefault(x => x.Language.Equals(store.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase));

                    //If we managed to decode in default language need to check if same value was not encoded in original language   
                    if (seoKeyword != null)
                    {
                        //If we are encodoing simply encode it in default language, because not found in original language
                        if (byValue)
                        {
                            return seoKeyword;
                        }

                        //If it was encoded in original language it should not allow to decode in default language.
                        //ex.: say in in english we have category keyword video with value tv-video and in russian there is also same value with keyword videoRussian
                        //If requested language is russian and keyword video we would get null, but default language (suppose is english) would return tv-video
                        var originalKeyword =
                            SeoKeywordClient.GetSeoKeywords(type, language, keywordvalue: seoKeyword.KeywordValue)
                                .FirstOrDefault();

                        if (originalKeyword == null)
                        {
                            return seoKeyword;
                        }
                    }
                }
            }

            return null;
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

        public static bool ChildOutputCacheEnabled
        {
            get
            {
                var retVal = true; // if there is no such setting we assume cache enabled

                var settings = GetSettings("ChildOutputCacheEnabled");

                if (settings != null && settings.Length > 0)
                {
                    retVal = settings.First().BooleanValue;
                }

                return retVal;
            }
        }
	}
}
