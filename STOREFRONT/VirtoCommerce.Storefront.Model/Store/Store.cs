using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent store - main ecommerce aggregate unit
    /// </summary>
    public class Store : Entity, IHasSettings
    {
        public Store()
        {
            Languages = new List<Language>();
            Currencies = new List<Currency>();
            SeoInfos = new List<SeoInfo>();
            DynamicProperties = new List<DynamicProperty>();
            Settings = new List<SettingEntry>();
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
        public StoreStatus StoreState { get; set; }

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

        /// <summary>
        /// All linked stores (their accounts can be reused here)
        /// </summary>
        public ICollection<string> TrustedGroups { get; set; }

        public bool QuotesEnabled
        {
            get
            {
                return Settings.GetSettingValue("Quotes.EnableQuotes", false);
            }
        }

        #region IhasSettings Member
        public ICollection<SettingEntry> Settings { get; set; }
        #endregion

        public SeoLinksType SeoLinksType { get; set; }

        //Need sync store currencies with system avail currencies for specific language
        public void SyncCurrencies(IEnumerable<Currency> availableCurrencies, Language language)
        {
            var allCurrencies = availableCurrencies.ToList();
            var newCurrencies = Currencies
                .Select(storeCurrency => allCurrencies.FirstOrDefault(c => c.Equals(storeCurrency)) ?? new Currency(language, storeCurrency.Code))
                .ToList();

            Currencies = newCurrencies;
            DefaultCurrency = Currencies.FirstOrDefault(c => c.Equals(DefaultCurrency)) ?? new Currency(language, DefaultCurrency.Code);
        }

        /// <summary>
        /// Checks if specified URL starts with store URL or store secure URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsStoreUrl(Uri url)
        {
            var result = false;

            var requestAddress = url.ToString();

            if (!string.IsNullOrEmpty(Url))
            {
                result = requestAddress.StartsWith(Url, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!result && !string.IsNullOrEmpty(SecureUrl))
            {
                result = requestAddress.StartsWith(SecureUrl, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }
    }
}
