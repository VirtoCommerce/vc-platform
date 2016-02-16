using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent store - main ecommerce aggregate unit
    /// </summary>
    public class Store : Entity
    {
        public Store()
        {
            Languages = new List<Language>();
            Currencies = new List<Currency>();
            SeoInfos = new List<SeoInfo>();
            DynamicProperties = new List<DynamicProperty>();

        }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Url of store storefront
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Secure url of store, must use https protocol, required
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// State of store (open, closing, maintenance)
        /// </summary>
        public string StoreState { get; set; }

        public string TimeZone { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        /// <summary>
        /// Default Language culture name  of store ( example en-US )
        /// </summary>
        public Language DefaultLanguage { get; set; }

        /// <summary>
        /// All supported lanuagages
        /// </summary>
        public ICollection<Language> Languages { get; set; }

        /// <summary>
        /// Default currency of store. 
        /// </summary>
        public Currency DefaultCurrency { get; set; }
        /// <summary>
        /// List of all supported currencies
        /// </summary>
        public ICollection<Currency> Currencies { get; set; }

        /// <summary>
        /// Product catalog id assigned to store
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Contact email of store
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Administrator contact email of store
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Store theme name
        /// </summary>
        public string ThemeName { get; set; }
  
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Seo info for current language
        /// </summary>
        public SeoInfo CurrentSeoInfo { get; set; }
        /// <summary>
        /// All store seo informations for all languages
        /// </summary>
        public ICollection<SeoInfo> SeoInfos { get; set; }

        public bool QuotesEnabled
        {
            get
            {
                bool isEnabled = false;

                var dynamicPropertyValue = DynamicProperties.GetDynamicPropertyValue("EnableQuotes");
                bool.TryParse(dynamicPropertyValue, out isEnabled);

                return isEnabled;
            }
        }

        //Need sync store currencies with system avail currencies for specific language
        public void SyncCurrencies(IEnumerable<Currency> availCurrencies, Language language)
        {
            var storeCurrencies = new List<Currency>();
            foreach (var storeCurrency in Currencies)
            {
                var currency = availCurrencies.FirstOrDefault(x => x.Equals(storeCurrency));
                if (currency == null)
                {
                    currency = new Currency(language, storeCurrency.Code);
                }
                storeCurrencies.Add(currency);
            }
            Currencies = storeCurrencies;
            DefaultCurrency = Currencies.FirstOrDefault(x => x.Equals(DefaultCurrency)) ?? new Currency(language, DefaultCurrency.Code);
        }

        /// <summary>
        /// Check that specified request uri matched to this store
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public bool IsStoreUri(Uri requestUri)
        {
            var retVal = false;
            if (requestUri.Scheme == Uri.UriSchemeHttps)
            {
                retVal = !String.IsNullOrEmpty(SecureUrl) && requestUri.ToString().StartsWith(SecureUrl);
            }
            else
            {
                retVal = !String.IsNullOrEmpty(Url) && requestUri.ToString().StartsWith(Url);
            }
            return retVal;
        }
    }
}