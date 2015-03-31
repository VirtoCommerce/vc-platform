using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts.Stores
{
    public class Store
    {
        #region Fields

        private IDictionary<string, object> _settings = new Dictionary<string, object>();

        #endregion

        #region Public Properties

        public string Catalog { get; set; }

        public string Country { get; set; }

        public string[] Currencies { get; set; }

        public string DefaultCurrency { get; set; }

        public string DefaultLanguage { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        public string[] Languages { get; set; }

        public string[] LinkedStores { get; set; }

        public string Name { get; set; }

        public string Region { get; set; }

        public string SecureUrl { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public IDictionary<string, object> Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public StoreState StoreState { get; set; }

        public string TimeZone { get; set; }

        public string Url { get; set; }

        #endregion

        #region Public Indexers

        [JsonIgnore]
        public object this[string name]
        {
            get { return _settings[name]; }
            set
            {
                if (_settings.ContainsKey(name))
                {
                    _settings[name] = value;
                }
                else
                {
                    _settings.Add(name, value);
                }
            }
        }

        #endregion
    }
}
