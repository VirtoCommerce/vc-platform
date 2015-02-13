namespace VirtoCommerce.ApiClient.DataContracts.Contents
{
    #region

    using System.Collections.Generic;

    using Newtonsoft.Json;

    #endregion

    public class DynamicContentItem
    {
        #region Fields

        private IDictionary<string, string> _properties = new Dictionary<string, string>();

        #endregion

        #region Public Properties

        public string ContentType { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        public bool IsMultilingual { get; set; }

        public string Name { get; set; }

        public IDictionary<string, string> Properties
        {
            get
            {
                return this._properties;
            }
            set
            {
                this._properties = value;
            }
        }

        #endregion

        #region Public Indexers

        [JsonIgnore]
        public string this[string name]
        {
            get
            {
                return this._properties[name];
            }
            set
            {
                if (this._properties.ContainsKey(name))
                {
                    this._properties[name] = value;
                }
                else
                {
                    this._properties.Add(name, value);
                }
            }
        }

        #endregion
    }
}