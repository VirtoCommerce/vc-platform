using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Store
    {
        #region Public Properties

        public string Catalog { get; set; }
        public string Country { get; set; }
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public CurrencyCodes[] Currencies { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes? DefaultCurrency { get; set; }
        public string DefaultLanguage { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public string[] Languages { get; set; }

        public string[] LinkedStores { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string SecureUrl { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public PropertyDictionary Settings { get; set; }
        //public StoreSetting[] Settings { get; set; }
        public int StoreState { get; set; }

        public string TimeZone { get; set; }
        public string Url { get; set; }

        public string State { get; set; }

        #endregion
    }
}
