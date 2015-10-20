using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Store
    {
        /// <summary>
        /// Gets or sets the value of store catalog id
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets the country name where store is located
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the collection of store currencies
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public CurrencyCodes[] Currencies { get; set; }

        /// <summary>
        /// Gets or sets the value of store default currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes? DefaultCurrency { get; set; }

        /// <summary>
        /// Gets or sets the value of store default language
        /// </summary>
        /// <value>
        /// Language culture name format (en-US etc)
        /// </value>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or sets the value of store description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the collection of store language codes
        /// </summary>
        /// <value>
        /// Culture name in ISO 3166-1 alpha-3 format
        /// </value>
        public string[] Languages { get; set; }

        /// <summary>
        /// Gets or sets the collection of linked stores
        /// </summary>
        /// <value>
        /// Stores ids
        /// </value>
        public string[] LinkedStores { get; set; }

        /// <summary>
        /// Gets or sets the value of store name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of region where store is located
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the value of store secure absolute URL
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets the collection of store SEO parameters
        /// </summary>
        /// <value>
        /// Array of SeoKeyword
        /// </value>
        public SeoKeyword[] Seo { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of store settings
        /// </summary>
        /// <value>
        /// Settings object
        /// </value>
        public PropertyDictionary Settings { get; set; }

        /// <summary>
        /// Gets or sets the numeric value of store current state
        /// </summary>
        /// <value>
        /// 0 - Opened, 1 - Closed, 2 - RestrictedAccess
        /// </value>
        public int StoreState { get; set; }

        /// <summary>
        /// Gets or sets the value of store time zone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the value of store absolute URL (HTTP)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the string value of store current state
        /// </summary>
        /// <value>
        /// "Open", "Closed", "RestrictedAccess"
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the collection of payment methods which are available for store
        /// </summary>
        /// <value>
        /// Array of PaymentMethod
        /// </value>
        public PaymentMethod[] PaymentMethods { get; set; }
    }
}